using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService;

namespace UzmanCrm.CrmService.WebAPI
{
    [System.Runtime.InteropServices.Guid("F0D32C65-E557-4AB6-A580-9AC4EE724EA2")]
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var services = new ServiceCollection();


            #region Redis için.
            try
            {
                bool isTest = false;
                Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
                var redisInfo = new string[5];
                if (isTest)
                {
                    var redisConfigValue = ConfigurationManager.AppSettings["Redis_Config"] != null
                        ? ConfigurationManager.AppSettings["Redis_Config"].ToString().Split(',')
                        : redisInfo = new string[1];
                    if (redisConfigValue.Length > 4)
                    {
                        var redisConfig = new ConfigurationOptions
                        {
                            EndPoints = { redisConfigValue[0] },
                            User = redisConfigValue[1], // Kullanıcı adı
                            Password = redisConfigValue[2], // Parola
                            Ssl = Convert.ToBoolean(redisConfigValue[3]), // SSL kullanımı (opsiyonel)
                            AbortOnConnectFail = Convert.ToBoolean(redisConfigValue[4]) // Bağlantı hatası durumunda davranış (opsiyonel)
                        };
                        services.AddStackExchangeRedisCache(_ =>
                            _.ConfigurationOptions = redisConfig
                            ).BuildServiceProvider();
                    }

                }
            }
            catch (Exception ex)
            {

            }

            #endregion


            var containerBuilder = new ContainerBuilder();
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterApplicationServicesAPI(containerBuilder);
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterAutoMapperProfile(containerBuilder);
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterNLog(containerBuilder);

            //redis için.
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();

            // If use project for mvc use bottom line
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // If use project for api use bottom line 
            var config = System.Web.Http.GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //var mapperConfiguration = container.Resolve<MapperConfiguration>();
            //mapperConfiguration.AssertConfigurationIsValid();

            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;

            //Trimming text values from all requests
            FormatterConfig.Register(config);

            #region STATIC_DATA

            IAddressService addressService;

            using (var scope = container.BeginLifetimeScope())
            {
                addressService = scope.Resolve<IAddressService>();
            }

            addressService.SetStaticCityList();
            addressService.SetStaticCountryList();
            addressService.SetStaticDistrictList();
            addressService.SetStaticNeighborhoodList();

            #endregion STATIC_DATA

        }

        protected void Application_PreSendRequestContent()
        {
            if (HttpContext.Current != null)
            {
                Response.Headers.Remove("X-Powered-By");
                Response.Headers.Remove("X-AspNet-Version");
                Response.Headers.Remove("X-AspNetMvc-Version");
                Response.Headers.Remove("Server");
            }
        }
    }
}
