using AutoMapper;
using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;
using System.Web.Http;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.Campaign;

namespace UzmanCrm.CrmService.WebAPI.Controllers
{
    public class CampaignController : ApiController
    {

        private readonly IMapper mapper;
        private readonly ILoginService loginService;
        private readonly ILogService logService;

        public CampaignController(IMapper mapper, ILoginService loginService, ILogService logService)
        {
            this.mapper = mapper;
            this.loginService = loginService;
            this.logService = logService;
        }

        /// <summary>
        /// Get customer and campaign info  (müşteri bilgileri,kampanya bilgileri ve puanını getirmek için...)
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,***** için kullanılır. 
        /// <br/>*******
        /// <br/>*******
        /// <br/>*******
        /// </remarks>
        /// <returns>Contact dto</returns>
        [HttpPost]
        //[SwaggerRequestExample(typeof(SearchCustomerRequest), typeof(SearchCustomerRequestExamples))]
        //[SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(SearchCustomerResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<GetCustomerCampaignInfoResponse>))]
        //[SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/campaign/get-customer-campaign-info")]
        public async Task<IHttpActionResult> GetCustomerCampaignInfoAsync(GetCustomerCampaignInfoRequest request)
        {
            var response = new Response<GetCustomerCampaignInfoResponse>();

            return Ok(response);
        }

        /// <summary>
        /// Run product campaign 
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,***** için kullanılır.  Müşteri Id bilgisi ile sepeti göndererek uygun kampanyaları kazandıran metot
        /// <br/>*******
        /// <br/>*******
        /// <br/>*******
        /// </remarks>
        /// <returns>Contact dto</returns>
        [HttpPost]
        //[SwaggerRequestExample(typeof(SearchCustomerRequest), typeof(SearchCustomerRequestExamples))]
        //[SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(SearchCustomerResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<RunProductCampaignResponse>))]
        //[SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/campaign/run-product-campaign")]
        public async Task<IHttpActionResult> RunProductCampaignAsync(RunProductCampaignRequest request)
        {
            var response = new Response<RunProductCampaignResponse>();

            return Ok(response);
        }



        /// <summary>
        /// Complete campaign process
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,***** için kullanılır.
        /// <br/>*******
        /// <br/>*******
        /// <br/>*******
        /// </remarks>
        /// <returns>Contact dto</returns>
        [HttpPost]
        //[SwaggerRequestExample(typeof(SearchCustomerRequest), typeof(SearchCustomerRequestExamples))]
        //[SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(SearchCustomerResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<object>))]
        //[SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/campaign/complete-campaign-process")]
        public async Task<IHttpActionResult> CompleteCampaignProcessAsync(CompleteCampaignProcessRequest request)
        {
            var response = new Response<object>();

            return Ok(response);
        }
    }
}
