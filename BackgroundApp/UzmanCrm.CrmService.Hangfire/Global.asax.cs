using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.WebApi;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System.Web;
using System.Web.Mvc;

namespace UzmanCrm.CrmService.Hangfire
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var services = new ServiceCollection();

            //redis için.
            services.AddStackExchangeRedisCache(_ => _.Configuration = "localhost:6379").BuildServiceProvider();

            var containerBuilder = new ContainerBuilder();
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterApplicationServicesAPI(containerBuilder);
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterAutoMapperProfile(containerBuilder);
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterNLog(containerBuilder);

            //redis için.
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            GlobalConfiguration.Configuration.UseAutofacActivator(container);

            ////// If use project for mvc use bottom line
            //////DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // If use project for api use bottom line 
            var config = System.Web.Http.GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //////var mapperConfiguration = container.Resolve<MapperConfiguration>();
            //////mapperConfiguration.AssertConfigurationIsValid();

            AreaRegistration.RegisterAllAreas();
            //System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;



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
