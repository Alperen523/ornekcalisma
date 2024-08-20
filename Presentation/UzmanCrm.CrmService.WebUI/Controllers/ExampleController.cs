using System.Threading.Tasks;
using System.Web.Mvc;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService;
using UzmanCrm.CrmService.Domain.Entity.CRM.Login;

namespace UzmanCrm.CrmService.WebUI.Controllers
{
    public class ExampleController : Controller
    {
        private readonly ILoginService loginService;

        public ExampleController(ILoginService loginService)
        {
            this.loginService = loginService;
        }
        // GET: Example
        public async Task<ActionResult> Index()
        {
            var request = new ApiUserLoginRequestDto { Password = "12345", Username = "crmapi" };
            var res = await loginService.Authenticate(request);

            return View();
        }
    }
}