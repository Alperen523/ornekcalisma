using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(UzmanCrm.CrmService.WebAPI.Startup))]

namespace UzmanCrm.CrmService.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }

    }
}
