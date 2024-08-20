using FluentValidation.WebApi;
using System.Web.Http;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.WebAPI.Validation;
using UzmanCrm.CrmService.WebAPI.Validation.Setting;

namespace UzmanCrm.CrmService.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region CORS hatası için...         
            //Normalde diğer projeler için gerekmeye bilir ama burada test ederken CORS hatasına düşmeyecek.       
            //var enableCorsAttribute = new System.Web.Http.Cors.EnableCorsAttribute("*",
            //    "Origin, Content-Type, Accept",
            //    "GET, PUT, POST, DELETE, OPTIONS");
            //config.EnableCors(enableCorsAttribute);
            #endregion

            config.MessageHandlers.Add(new TokenValidationHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ValidateModelStateFilter());
            config.MessageHandlers.Add(new ResponseWrappingHandler());
            FluentValidationModelValidatorProvider.Configure(config);

        }
    }
}
