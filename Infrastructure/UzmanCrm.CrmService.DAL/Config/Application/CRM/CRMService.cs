using Autofac;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.RedisService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Application.Common;
using UzmanCrm.CrmService.DAL.Config.Application.CRM.Model;

namespace UzmanCrm.CrmService.DAL.Config.Application.CRM
{
    public class CRMService : ICRMService
    {
        private IXRMConfigService _xrmConfigService;
        private readonly ILogService logService;
        private readonly IRedisService redisService;

        public CRMService(
            IXRMConfigService xrmConfigService,
            ILogService logService,
            IRedisService redisService)
        {
            _xrmConfigService = xrmConfigService;
            this.logService = logService;
            this.redisService = redisService;
        }

        public void RefreshXrmService()
        {
            XRMConfigService.time_instance = XRMConfigService.time_instance.AddMinutes(-11);
            var builder = new ContainerBuilder();
            builder.RegisterType<XRMConfigService>().As<IXRMConfigService>();
            var container = builder.Build();
            container.Resolve<IXRMConfigService>();
        }

        /// <summary>
        /// Create and update entity
        /// Example entityName=account if inside model accountid property is not null it's mean update
        /// Example entityName=account if inside model accountid property is null it's mean create
        /// </summary>
        /// <typeparam name="TReq">Model type</typeparam>
        /// <param name="req">Model</param>
        /// <param name="entityName">Entity name(if cant send it's mean model name=entity name)</param>
        /// <returns></returns>
        public Response<Guid> Save<TReq>(TReq req, string entityName = null, string customeridtype = CrmService.Common.CommonStaticConsts.CustomerIdType.Contact, CompanyEnum co = CompanyEnum.KD)
        {
            var tryCount = 0;
            retry:

            var service = _xrmConfigService.GetService();
            if (service == null)
                throw new Exception("Save: There's no available service found");

            if (entityName == null)
            {
                entityName = req.GetType().Name;
            }
            var entity = new Entity();
            var res = new Response<Guid>();
            if (req.GetType().Name != "Entity")
            {
                entity = ModelToEntity(req, ref service, entityName, customeridtype, co);
            }
            else
            {
                entity = req as Entity;
            }

            var srvResponse = Guid.Empty;
            if (entity.Id != Guid.Empty && entity.Id != null)
            {
                try
                {
                    if (co == CompanyEnum.KD)
                        service.Update(entity);
                    else
                        _xrmConfigService.OrganizationServiceLT.Update(entity);
                    srvResponse = entity.Id;
                }
                catch (Exception ex)
                {
                    logService.LogSaveSync(CrmService.Common.Enums.LogEventEnum.DbError,
                        this.GetType().Name,
                        "CrmServiceUpdate",
                        co,
                        LogTypeEnum.Response,
                        ex);
                    CommonMethod.SetError(res, ex.Message, true);
                    tryCount++;
                    if (CommonHelper.GoToTag(ex.Message))
                        goto retry;
                }
            }
            else
            {

                try
                {
                    if (co == CompanyEnum.KD)
                        srvResponse = service.Create(entity);
                    else
                        srvResponse = _xrmConfigService.OrganizationServiceLT.Create(entity);
                }
                catch (Exception ex)
                {
                    var ExMessage = "";
                    if (entity.Attributes.Contains("contactid"))
                        ExMessage = "contactid = " + entity["contactid"].ToString() + " - ";

                    logService.LogSaveSync(CrmService.Common.Enums.LogEventEnum.DbError,
                        this.GetType().Name,
                        "CrmServiceSave",
                        co,
                        LogTypeEnum.Response,
                        ExMessage + ex);
                    CommonMethod.SetError(res, ex.Message, true);
                    tryCount++;
                    if (CommonHelper.GoToTag(ex.Message))
                        goto retry;

                }
            }

            _xrmConfigService.ReleaseService(service);

            if (srvResponse != null && srvResponse != Guid.Empty)
            {
                res.Success = true;
                res.Data = srvResponse;
                res.Message = "Process completed.";
            }

            return res;
        }

        /// <summary>
        /// Delete entity by name or model name
        /// </summary>
        /// <typeparam name="TReq">Entity type</typeparam>
        /// <param name="req">Request entity</param>
        /// <param name="entityName">Entity name(if cant send it's mean model name=entity name)</param>
        public void Delete<TReq>(TReq req, string entityName = null, CompanyEnum co = CompanyEnum.KD)
        {
            var service = _xrmConfigService.GetService();
            if (service == null)
                throw new Exception("Delete: There's no available service found");

            if (entityName != null)
            {
                entityName = req.GetType().Name;
            }
            var id = req.GetType().GetProperty(entityName + "id");
            if (id != null)
            {
                if (co == CompanyEnum.KD)
                    service.Delete(entityName, (Guid)id.GetValue(req));
                else
                    _xrmConfigService.OrganizationServiceLT.Delete(entityName, (Guid)id.GetValue(req));
            }
            _xrmConfigService.ReleaseService(service);
        }

        /// <summary>
        /// Getting crm entity for update,delete and create
        /// </summary>
        /// <typeparam name="TReq">Entity type</typeparam>
        /// <param name="model">Request entity</param>
        /// <param name="entityName">Entity name(if cant send it's mean model name=entity name)</param>
        /// <returns>Crm entity</returns>
        private Entity ModelToEntity<T>(T model, ref IOrganizationService service, string entityName = null, string customeridtype = CrmService.Common.CommonStaticConsts.CustomerIdType.Contact, CompanyEnum co = CompanyEnum.KD)
        {
            EntityMetadata entityMetadata = null;
            EntityMetadata_Temp entityMetadata_Temp = null;
            var customeridtypeValue = model.GetType().GetProperty("customeridtype") != null ? model.GetType().GetProperty("customeridtype").GetValue(model) : null;
            customeridtypeValue = !customeridtypeValue.IsNotNullAndEmpty() ? customeridtype : (customeridtype == "1" ? "account" : "contact");

            if (entityName == null)
            {
                entityName = model.GetType().Name;
            }

            entityMetadata = redisService.GetItem<EntityMetadata>(entityName + "_" + co.ToString());

            try
            {
                if (entityMetadata == null)
                {
                    if (co == CompanyEnum.KD)
                        entityMetadata = _xrmConfigService.GetEntityMetaData(entityName, CompanyEnum.KD, ref service);
                    else
                        entityMetadata = _xrmConfigService.GetEntityMetaData(entityName, CompanyEnum.LT, ref service);

                    redisService.SetItem(entityMetadata, entityName + "_" + co.ToString(), 24, 24, false);
                }

            }
            catch (Exception ex)
            {
                logService.LogSaveSync(CrmService.Common.Enums.LogEventEnum.DbError,
                      this.GetType().Name,
                      "CrmServiceModelToEntity",
                      co,
                      LogTypeEnum.Response,
                      ex);
            }


            var entity = new Entity(entityName);
            foreach (var item in model.GetType().GetProperties())
            {
                try
                {
                    var propName = item.Name;
                    var propValue = item.GetValue(model);
                    var attributeItem = entityMetadata.Attributes.FirstOrDefault(x => x.LogicalName == propName);
                    if (attributeItem != null && propValue != null)//this property has a crm
                    {
                        DetermineFormType(propValue, attributeItem, entity, customeridtypeValue.ToString());
                    }
                }
                catch
                {
                    continue;
                }
            }

            return entity;
        }

        /// <summary>
        /// Determine form type and fill Entity
        /// </summary>
        /// <param name="value">Model value</param>
        /// <param name="attributeMetaData">Attribute metadata(attribute name is LogicalName)</param>
        /// <param name="entity">Crm entity</param>
        private void DetermineFormType(object value, AttributeMetadata attributeMetaData, Entity entity, string CustomerIdType = CrmService.Common.CommonStaticConsts.CustomerIdType.Contact)
        {

            var type = attributeMetaData.AttributeType.ToString();
            Object res = null;
            switch (type)
            {
                case "BigInt":
                    if (value.GetType().IsArray)
                        res = BitConverter.ToInt32((byte[])value, 0);
                    else
                        res = Convert.ToInt32(value);
                    break;
                case "Boolean":
                    res = Convert.ToBoolean(value);
                    break;
                case "CalendarRules":
                    if ((DateTime)value == DateTime.MinValue)
                        break;
                    res = Convert.ToDateTime(value.ToString());
                    break;
                case "Customer":
                    if ((Guid)value == Guid.Empty)
                        break;
                    var eRef = ((LookupAttributeMetadata)attributeMetaData)
                    .Targets.Where(x => x == CustomerIdType)
                    .Except(((LookupAttributeMetadata)attributeMetaData).Targets.Where(x => x == attributeMetaData.EntityLogicalName)).ToList();

                    res = new EntityReference(eRef[0], (Guid)value);
                    //CustomerIdType 1 = account , 2 = contact
                    break;
                case "DateTime":
                    if ((DateTime)value == DateTime.MinValue)
                        break;
                    res = Convert.ToDateTime(value.ToString());
                    break;
                case "Decimal":
                    res = Convert.ToDecimal(value);
                    break;
                case "Double":
                    res = Convert.ToDouble(value);
                    break;
                case "EntityName":
                    res = Convert.ToInt32(value);
                    break;
                case "Integer":
                    res = Convert.ToInt32(value);
                    break;
                case "Lookup":
                    if ((Guid)value == Guid.Empty)
                        break;
                    var eRefL = ((LookupAttributeMetadata)attributeMetaData)
                    .Targets
                    .ToList()
                    .Except(((LookupAttributeMetadata)attributeMetaData).Targets.ToList().Where(x => x == attributeMetaData.EntityLogicalName)
                    .ToList()).ToList();
                    res = new EntityReference(eRefL[0], (Guid)value);
                    break;
                //case "ManagedProperty":
                // res = DT_STRING;
                // break;
                case "Memo":
                    res = value;
                    break;
                case "Money":
                    res = new Money(Convert.ToDecimal(value));
                    break;
                //case "Owner":
                // res = DT_STRING;
                // break;
                //case "PartyList":
                // res = DT_STRING;
                // break;
                case "Picklist":
                    if ((int)value < 0)
                        break;
                    res = new OptionSetValue(Convert.ToInt32(value));
                    break;
                case "State":
                    res = new OptionSetValue(Convert.ToInt32(value));
                    break;
                case "Status":
                    res = new OptionSetValue(Convert.ToInt32(value));
                    break;
                case "String":
                    if ((string)value == "" || value == null)
                        break;
                    res = value.ToString();
                    break;
                case "Uniqueidentifier":
                    res = (Guid)value;
                    entity.Id = (Guid)value;
                    break;
                default:
                    break;
            }
            if (res != null)
                entity.Attributes.Add(attributeMetaData.LogicalName, res);
        }

        public Guid SendCrmEmail(string[] ToGuids, string ToType, string Subject, string Body, List<string> toMail = null, List<string> ccMail = null, object attachment = null, string attachtype = "", string attachname = "")
        {
            var tryCount = 0;
            retry:

            var service = _xrmConfigService.GetService();
            try
            {

                Entity email = new Entity();
                EntityReference from = new EntityReference("systemuser", new Guid("A627B337-47CD-E811-8120-005056991930"));
                Entity e_from = new Entity("activityparty");
                e_from.Attributes.Add("partyid", from);
                EntityCollection ec_FromParty = new EntityCollection();
                ec_FromParty.EntityName = "systemuser";
                ec_FromParty.Entities.Add(e_from);
                EntityCollection ec_ToParty = new EntityCollection();
                ec_ToParty.EntityName = ToType;
                if (ToGuids[0].IsNotNullAndEmpty())
                {
                    for (int i = 0; i < ToGuids.Length; i++)
                    {
                        EntityReference to = new EntityReference(ToType, new Guid(ToGuids[i]));
                        Entity e_to = new Entity("activityparty");
                        e_to.Attributes.Add("partyid", to);
                        ec_ToParty.Entities.Add(e_to);
                    }
                    email.Attributes.Add("to", ec_ToParty);
                }

                EntityCollection toList = new EntityCollection();
                if (toMail != null && toMail.Count() > 0)
                {
                    foreach (var eMail in toMail)
                    {
                        Entity to1 = new Entity("activityparty");
                        to1["addressused"] = eMail;
                        if (eMail != "")
                            toList.Entities.Add(to1);
                    }
                    email.Attributes.Add("to", toList);
                }
                EntityCollection ccList = new EntityCollection();
                if (ccMail != null && ccMail.Count() > 0)
                {
                    foreach (var eMail in ccMail)
                    {
                        Entity to1 = new Entity("activityparty");
                        to1["addressused"] = eMail;
                        if (eMail != "")
                            ccList.Entities.Add(to1);
                    }
                    email.Attributes.Add("cc", ccList);
                }

                email.LogicalName = "email";
                email.Attributes.Add("from", ec_FromParty);
                email.Attributes.Add("subject", Subject);
                email.Attributes.Add("description", Body);
                Guid EmailGuid = service.Create(email);
                if (attachment != null)
                {
                    string encodedData = Convert.ToBase64String(attachment as byte[]);

                    Entity at = new Entity("activitymimeattachment");
                    at["objectid"] = new EntityReference("email", EmailGuid);
                    at["objecttypecode"] = "email";
                    at["subject"] = Subject;
                    if (!String.IsNullOrEmpty(encodedData)) at["body"] = encodedData;
                    at["filename"] = attachname;
                    at["mimetype"] = attachtype;

                    Guid idn = service.Create(at);
                }
                SendEmailRequest sendEmail = new SendEmailRequest();
                sendEmail.EmailId = EmailGuid;
                sendEmail.IssueSend = true;
                sendEmail.TrackingToken = "";
                SendEmailResponse response = (SendEmailResponse)service.Execute(sendEmail);

                _xrmConfigService.ReleaseService(service);
                return EmailGuid;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                tryCount++;
                if ((ex.Message.Includes("failed.") || ex.Message.Includes("System.ServiceModel.Channel") || ex.Message.Includes("timeout")) && tryCount < 2)
                    goto retry;
                string m = ex.Detail.Message.ToString();

                _xrmConfigService.ReleaseService(service);
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                tryCount++;
                if ((ex.Message.Includes("failed.") || ex.Message.Includes("System.ServiceModel.Channel") || ex.Message.Includes("timeout")) && tryCount < 2)
                    goto retry;
                string m = ex.ToString();

                _xrmConfigService.ReleaseService(service);
                return Guid.Empty;
            }
        }

        ////TODO:
        ///// <summary>
        ///// Retry for entities that fail while creating and updating
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="co"></param>
        ///// <param name="createType"></param>
        ///// <returns></returns>
        //private Response<Guid> ReSave(Entity entity, CompanyEnum co, CreateType createType)
        //{
        //    var res = new Response<Guid>();
        //    var srvResponse = Guid.Empty;

        //    IXRMConfigService _xrmConfigService;
        //    IXRMConfigServiceVakko _xrmConfigServiceVakko;

        //    var builder = new ContainerBuilder();
        //    builder.RegisterType<XRMConfigService>().As<IXRMConfigService>();
        //    builder.RegisterType<XRMConfigServiceVakko>().As<IXRMConfigServiceVakko>();
        //    var container = builder.Build();

        //    using (var scope = container.BeginLifetimeScope())
        //    {
        //        _xrmConfigService = scope.Resolve<IXRMConfigService>();
        //        _xrmConfigServiceVakko = scope.Resolve<IXRMConfigServiceVakko>();
        //    }


        //    if (co == CompanyEnum.CL)
        //    {
        //        if (createType == CreateType.Create)
        //            srvResponse = _xrmConfigService.OrganizationService.Create(entity);
        //        else
        //            _xrmConfigService.OrganizationService.Update(entity);
        //    }
        //    else
        //    {
        //        if (createType == CreateType.Create)
        //            srvResponse = _xrmConfigServiceVakko.OrganizationService.Create(entity);
        //        else
        //            _xrmConfigServiceVakko.OrganizationService.Update(entity);
        //    }


        //    return res;
        //}

        ////TODO:
        ///// <summary>
        ///// Retry for entities that fail while delete
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="co"></param>
        ///// <param name="createType"></param>
        ///// <param name="entityName"></param>
        ///// <returns></returns>
        //private Response<Guid> ReDelete(Entity entity, CompanyEnum co, CreateType createType, string entityName)
        //{
        //    var res = new Response<Guid>();
        //    var srvResponse = Guid.Empty;

        //    IXRMConfigService _xrmConfigService;
        //    IXRMConfigServiceVakko _xrmConfigServiceVakko;

        //    var builder = new ContainerBuilder();
        //    builder.RegisterType<XRMConfigService>().As<IXRMConfigService>();
        //    builder.RegisterType<XRMConfigServiceVakko>().As<IXRMConfigServiceVakko>();
        //    var container = builder.Build();

        //    using (var scope = container.BeginLifetimeScope())
        //    {
        //        _xrmConfigService = scope.Resolve<IXRMConfigService>();
        //        _xrmConfigServiceVakko = scope.Resolve<IXRMConfigServiceVakko>();
        //    }

        //    if (entityName != null)
        //    {
        //        entityName = entity.GetType().Name;
        //    }
        //    var id = entity.GetType().GetProperty(entityName + "id");
        //    if (id.IsNotNullAndEmpty())
        //    {
        //        if (co == CompanyEnum.CL)
        //            _xrmConfigService.OrganizationService.Delete(entityName, (Guid)id.GetValue(entity));
        //        else
        //            _xrmConfigServiceVakko.OrganizationService.Delete(entityName, (Guid)id.GetValue(entity));
        //    }

        //    return res;
        //}
    }
}
