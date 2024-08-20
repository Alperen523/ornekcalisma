using AutoMapper;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Domain.Entity.CRM.Login;
using UzmanCrm.CrmService.WebAPI.Examples.Response;
using UzmanCrm.CrmService.WebAPI.Models.Login;

namespace UzmanCrm.CrmService.WebAPI.Controllers
{
    public class LoginController : ApiController
    {

        private readonly IMapper mapper;
        private readonly ILoginService loginService;
        private readonly ILogService logService;

        public LoginController(IMapper mapper, ILoginService loginService, ILogService logService)
        {
            this.mapper = mapper;
            this.loginService = loginService;
            this.logService = logService;
        }

        /// <summary>
        ///  Authenticate
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,Diğer metotlara güvenli giriş sağlayabilmek için kullanılır. 
        /// <br/>API erişiminde güvenlik için Kimlik Doğrulaması (“Bearer Token”) yöntemi kullanılmaktadır. Bu yöntemde, istemci ile API arasında taşınan tokenler ile kimlik doğrulaması sağlanır
        /// </remarks>
        /// <returns>TokenResponse</returns>
        [HttpPost]
        [Route("api/login/authenticate")]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(TokenResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<TokenResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<TokenResponse>))]
        public async Task<IHttpActionResult> Authenticate(LoginRequest model)
        {
            //await logService.LogSave(LogEventEnum.DbInfo, this.GetType().Name, nameof(Authenticate), CompanyEnum.VK, LogTypeEnum.Request, model);
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var mappingModel = mapper.Map<LoginRequest, ApiUserLoginRequestDto>(model);
            var resp = await loginService.Authenticate(mappingModel);
            stopwatch.Stop();

            if (resp != null)
                resp.Data.Loginfo = stopwatch.Elapsed.Milliseconds.ToString() + " Milliseconds";


            //await logService.LogSave(LogEventEnum.DbInfo, this.GetType().Name, nameof(Authenticate), CompanyEnum.KD, LogTypeEnum.Response, resp);

            return Ok(resp);



        }

    }
}