using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel.Description;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Application.CRM.Model;

namespace UzmanCrm.CrmService.DAL.Config.Application.CRM
{
    public class XRMConfigService : IXRMConfigService
    {
        private static ConcurrentBag<CrmServiceWrapper> staticServicePool = new ConcurrentBag<CrmServiceWrapper>();
        #region | DEFINATIONS |

        private static IOrganizationService organizationServiceVakko;
        private static IOrganizationService organizationServiceLatelier;
        private static OrganizationServiceContext organizationServiceContextVakko;
        private static OrganizationServiceContext organizationServiceContextLatelier;
        private static object lockobject = new object();
        public static DateTime time_instance;
        private readonly ILogService _logService;

        public XRMConfigService(ILogService logService)
        {
            _logService = logService;

        }

        #region Service Pool

        public IOrganizationService GetService([CallerMemberName] string callerMethodName = null)
        {
            lock (staticServicePool)
            {
                if (!staticServicePool.IsNotNullAndEmpty())
                {
                    InitializeStaticServicePool();
                }

                // Bulunabilir durumda olan ilk servisi al
                var availableService = staticServicePool.OrderBy(x => x.TimeInstance).FirstOrDefault(s => !s.IsInUse);

                if (availableService != null)
                {
                    // Servisin son kullanılma zamanını kontrol et
                    var timeSpan = DateTime.Now.Subtract(availableService.TimeInstance);
                    if (timeSpan.TotalMinutes >= 30)
                        RefreshService(availableService);
                    // Servisi kullanımda olarak işaretle
                    availableService.IsInUse = true;
                    availableService.UsedDate = DateTime.Now;
                    availableService.CallerMethodName = callerMethodName;
                    return availableService.Service;
                }
                else
                {
                    var serialized = SerializeServiceList(staticServicePool);

                    _logService.LogSave(LogEventEnum.DbError,
                                 this.GetType().Name,
                                 nameof(GetService),
                                 CompanyEnum.KD,
                                 LogTypeEnum.Response,
                                 serialized
                             );

                    // Tüm servisler şu anda kullanımda
                    availableService = staticServicePool.OrderBy(x => x.UsedDate).FirstOrDefault();
                    // Servisin son kullanılma zamanını kontrol et
                    var timeSpan = DateTime.Now.Subtract(availableService.TimeInstance);
                    if (timeSpan.TotalMinutes >= 30)
                        RefreshService(availableService);
                    // Servisi kullanımda olarak işaretle
                    availableService.IsInUse = true;
                    availableService.UsedDate = DateTime.Now;
                    availableService.CallerMethodName = callerMethodName;
                    return availableService.Service;
                }
            }
        }

        private string SerializeServiceList(ConcurrentBag<CrmServiceWrapper> staticServicePool)
        {
            var serviceList = new ConcurrentBag<CrmServiceWrapper>();
            foreach (var service in staticServicePool)
            {
                var serv = new CrmServiceWrapper();
                serv.IsInUse = service.IsInUse;
                serv.UsedDate = service.UsedDate;
                serv.TimeInstance = service.TimeInstance;
                serv.CallerMethodName = service.CallerMethodName;
                serviceList.Add(serv);
            }
            var serialized = serviceList.JsonSerializeObject();
            return serialized;
        }

        public void ReleaseService(IOrganizationService releasedService)
        {
            lock (staticServicePool)
            {
                // İlgili statik servis sarıcısını bul
                var serviceWrapper = staticServicePool.FirstOrDefault(s => s.Service == releasedService);

                if (serviceWrapper != null)
                {
                    // Servisi kullanımda olmayacak şekilde işaretle
                    serviceWrapper.IsInUse = false;
                    serviceWrapper.UsedDate = null;
                    serviceWrapper.CallerMethodName = null;
                }
            }
        }

        private void InitializeStaticServicePool()
        {
            var serviceCount = ConfigurationManager.AppSettings["ServicePoolCount"].IsNotNullAndEmpty() ? ConfigurationManager.AppSettings["ServicePoolCount"].ToInt() : 50;
            staticServicePool = new ConcurrentBag<CrmServiceWrapper>();
            var serviceCountToAdd = serviceCount - staticServicePool.Count;
            for (int i = 0; i < serviceCountToAdd; i++)
            {
                AddStaticService();
            }
        }

        private void AddStaticService()
        {
            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            DefineOrganizationService(isTest ? ConfigurationManager.AppSettings["CRMOrganizationService_TEST"] : ConfigurationManager.AppSettings["CRMOrganizationService"], out organizationServiceVakko, out organizationServiceContextVakko);

            //DefineOrganizationService(ValidationHelper.GetCrmServiceUrl(), ref organizationService, out var context);

            // Servisi temsil eden sarıcı sınıfı oluştur
            var serviceWrapper = new CrmServiceWrapper
            {
                Service = organizationServiceVakko,
                IsInUse = false,
                UsedDate = null,
                CallerMethodName = null,
                TimeInstance = DateTime.Now
            };

            staticServicePool.Add(serviceWrapper);
        }

        private void RefreshService(CrmServiceWrapper serviceWrapper)
        {
            var service = serviceWrapper.Service;
            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            DefineOrganizationService(isTest ? ConfigurationManager.AppSettings["CRMOrganizationService_TEST"] : ConfigurationManager.AppSettings["CRMOrganizationService"], out organizationServiceVakko, out organizationServiceContextVakko);
            //DefineOrganizationService(ValidationHelper.GetCrmServiceUrl(), ref service, out var context);
            serviceWrapper.Service = service;
            serviceWrapper.IsInUse = false;
            serviceWrapper.UsedDate = null;
            serviceWrapper.TimeInstance = DateTime.Now;
        }

        #endregion

        private void GetServiceVakko()
        {
            if (organizationServiceVakko == null)
            {
                lock (lockobject)
                {
                    bool isTest = false;
                    Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
                    DefineOrganizationService(isTest ? ConfigurationManager.AppSettings["CRMOrganizationService_TEST"] : ConfigurationManager.AppSettings["CRMOrganizationService"], out organizationServiceVakko, out organizationServiceContextVakko);
                    time_instance = DateTime.Now;
                }
            }
            else
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(time_instance);
                if (timeSpan.Minutes >= 10)
                {
                    lock (lockobject)
                    {
                        bool isTest = false;
                        Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
                        DefineOrganizationService(isTest ? ConfigurationManager.AppSettings["CRMOrganizationService_TEST"] : ConfigurationManager.AppSettings["CRMOrganizationService"], out organizationServiceVakko, out organizationServiceContextVakko);
                        time_instance = DateTime.Now;
                    }
                }
            }
        }

        private void GetServiceLatelier()
        {
            if (organizationServiceLatelier == null)
            {
                lock (lockobject)
                {
                    bool isTest = false;
                    Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
                    DefineOrganizationService(isTest ? ConfigurationManager.AppSettings["CRMOrganizationServiceLatelier_TEST"] : ConfigurationManager.AppSettings["CRMOrganizationServiceLatelier"], out organizationServiceLatelier, out organizationServiceContextLatelier);
                    time_instance = DateTime.Now;
                }
            }
            else
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(time_instance);
                if (timeSpan.Minutes >= 10)
                {
                    lock (lockobject)
                    {
                        bool isTest = false;
                        Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
                        DefineOrganizationService(isTest ? ConfigurationManager.AppSettings["CRMOrganizationServiceLatelier_TEST"] : ConfigurationManager.AppSettings["CRMOrganizationServiceLatelier"], out organizationServiceLatelier, out organizationServiceContextLatelier);
                        time_instance = DateTime.Now;
                    }
                }
            }
        }

        private void DefineOrganizationService(string orgSvc, out IOrganizationService service, out OrganizationServiceContext context)
        {
            Uri myUri = new Uri(orgSvc);

            ClientCredentials myClientCredentials = new ClientCredentials();
            myClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["CRMUsername"];
            myClientCredentials.UserName.Password = ConfigurationManager.AppSettings["CRMPassword"];

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(myUri, null, myClientCredentials, null))
            {
                serviceProxy.Timeout = new TimeSpan(0, 34560, 0);
                service = serviceProxy;
                context = new OrganizationServiceContext(service);
            }

            // Bu kısım servis bir an önce toparlansın diye konuldu.
            // Servis ilk oluşturulduğunda çalışır halde olmuyor. İlk kullanımdan sonra çalışır hale geliyor
            try
            {
                var query = new QueryExpression("discount");
                query.TopCount = 1;
                query.ColumnSet = new ColumnSet("discountid");
                var c = service.RetrieveMultiple(query);
            }
            catch (Exception ex)
            {
            }
        }

        public IOrganizationService OrganizationServiceVakko => organizationServiceVakko;
        public IOrganizationService OrganizationServiceLT => organizationServiceLatelier;

        public OrganizationServiceContext OrganizationServiceContextVakko => organizationServiceContextVakko;
        public OrganizationServiceContext OrganizationServiceContextLatelier => organizationServiceContextLatelier;

        #endregion | DEFINATIONS |


        #region | Helper Functions |

        public EntityMetadata GetEntityMetaData(string entityName, CompanyEnum company, ref IOrganizationService service)
        {
            RetrieveEntityResponse res = null;
            var tryCount = 0;
            retry:
            try
            {
                RetrieveEntityRequest req = new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.All,
                    LogicalName = entityName, // like "account"
                    RetrieveAsIfPublished = true
                };
                if (company == CompanyEnum.KD)
                    res = (RetrieveEntityResponse)service.Execute(req);
                else
                    res = (RetrieveEntityResponse)organizationServiceLatelier.Execute(req);
            }
            catch (Exception ex)
            {
                tryCount++;
                if ((ex.Message.Includes("failed.") || ex.Message.Includes("System.ServiceModel.Channel") || ex.Message.Includes("timeout")) && tryCount < 2)
                    goto retry;
            }

            return res.EntityMetadata;
        }

        //memoryCache özelliği için kullanılabilir.
        //public EntityMetadata GetEntityMetaData(string entityName, CompanyEnum company)
        //{
        //    var hasMetadata = _memoryCache.TryGetValue("metadata-" + entityName, out MetadataCacheModel metadata);
        //    if (!hasMetadata || metadata.EntityMetadata == null || metadata.LastUpdatedDate < DateTime.Now.AddDays(-3))
        //    {
        //        RetrieveEntityRequest req = new RetrieveEntityRequest
        //        {
        //            EntityFilters = EntityFilters.All,
        //            LogicalName = entityName, // like "account"
        //            RetrieveAsIfPublished = true
        //        };

        //        RetrieveEntityResponse res = null;
        //        if (company == CompanyEnum.VK)
        //            res = (RetrieveEntityResponse)organizationServiceVakko.Execute(req);
        //        else
        //            res = (RetrieveEntityResponse)organizationServiceLatelier.Execute(req);

        //        if (res.EntityMetadata != null)
        //        {
        //            metadata = new MetadataCacheModel()
        //            {
        //                EntityName = entityName,
        //                EntityMetadata = res.EntityMetadata,
        //                LastUpdatedDate = DateTime.Now
        //            };
        //        }
        //        _memoryCache.Set("metadata-" + entityName, metadata);
        //    }
        //    else
        //    {
        //        metadata = _memoryCache.Get<MetadataCacheModel>("metadata-" + entityName);
        //    }
        //    return metadata?.EntityMetadata;
        //}

        #endregion | Helper Functions |
    }
}

