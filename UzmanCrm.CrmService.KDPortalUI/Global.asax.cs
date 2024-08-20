using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UzmanCrm.CrmService.KDPortalUI.Filters;


namespace UzmanCrm.CrmService.KDPortalUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var services = new ServiceCollection();

            //redis için.
            services.AddStackExchangeRedisCache(_ => _.Configuration = "localhost:6379").BuildServiceProvider();

            var containerBuilder = new ContainerBuilder();
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterApplicationServicesUI(containerBuilder);
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterAutoMapperProfile(containerBuilder);
            Infrastructure.Extensions.ServiceCollectionExtensions.RegisterNLog(containerBuilder);

            //redis için.
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            // If use project for mvc use bottom line
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // If use project for api use bottom line 
            //var config = GlobalConfiguration.Configuration;
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            //var mapperConfiguration = container.Resolve<MapperConfiguration>();
            //mapperConfiguration.AssertConfigurationIsValid();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MvcHandler.DisableMvcResponseHeader = true;
            GlobalFilters.Filters.Add(new JsonResultFilter { MaxJsonLength = int.MaxValue });



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
