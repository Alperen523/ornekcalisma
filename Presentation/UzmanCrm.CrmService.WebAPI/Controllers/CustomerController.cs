using AutoMapper;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Examples.Request.Contact;
using UzmanCrm.CrmService.WebAPI.Examples.Response.Contact;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ILoginService loginService;
        private readonly ILogService logService;
        private readonly IContactService contactService;



        public CustomerController(IMapper mapper, ILoginService loginService, ILogService logService, IContactService contactService)
        {
            this.mapper = mapper;
            this.loginService = loginService;
            this.logService = logService;
            this.contactService = contactService;
        }

        /// <summary>
        /// Get customer
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,müşteri bilgilerini almak için kullanılır. 
        /// <br/>MobilePhone değerine göre gelen data gösterilir.
        /// </remarks>
        /// <returns>GetCustomerResponse</returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetCustomerRequest), typeof(GetCustomerRequestExamples))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(GetCustomerResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<List<GetCustomerResponse>>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/customer/get")]
        public async Task<IHttpActionResult> GetCustomerAsync(GetCustomerRequest request)
        {
            var mappingModel = mapper.Map<GetCustomerRequest, GetCustomerRequestDto>(request);
            var response = await contactService.GetCustomerSearchAsync(mappingModel);

            var mappingResponse = mapper.Map<Response<GetCustomerResponseDto>, ResponseApi<GetCustomerResponse>>(response);

            if (mappingResponse.Error is not null)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }


        /// <summary>
        /// Save customer
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,eticaret sistemi için müşteri oluşturmak ve değiştirmek için kullanılır.İlgili kayıt içeride var ise güncelleştirir yok ise yeni kayıt olarak ekler.
        /// <br/>Örneğin;Model içerisindeki ad-soyad-gsm numarası veritabanında var ise db içerisinden ilgili kaydı bulur request deki yeni değerleri günceller.
        /// </remarks>
        /// <returns>Contact dto</returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(SaveCustomerRequest), typeof(SaveCustomerRequestExamples))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(SaveCustomerResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<SaveCustomerResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/customer/save")]
        public async Task<IHttpActionResult> SaveCustomerAsync(SaveCustomerRequest request)
        {
            //await logService.LogSave(LogEventEnum.DbInfo, "Request", nameof(SaveCustomerAsync), CompanyEnum.KD, LogTypeEnum.Request, request);
            Stopwatch stopwatch = Stopwatch.StartNew();

            var resp = new Response<SaveCustomerResponse>();
            var mappingModel = mapper.Map<SaveCustomerRequest, SaveCustomerRequestDto>(request);
            var response = await contactService.SaveCustomerAsync(mappingModel);
            var mappingResponse = mapper.Map<Response<SaveCustomerResponseDto>, ResponseApi<SaveCustomerResponse>>(response);

            //await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(SaveCustomerAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            stopwatch.Stop();

            mappingResponse.Message = mappingResponse.Message + " Milliseconds : " + stopwatch.ElapsedMilliseconds;

            if (mappingResponse.Error is not null)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        /// Update mobilapp
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,Web üzerinden kayıt olan bir müşterinin daha sonra mobil uygulamayı indirip login olduğunda, crm sistemine mobil uygulama id ve mobil uygulama indirme tarihi aktarılması için tasarlanmıştır.
        /// <br/>Örneğin;Web üzerinden kayıt olan bir müşteri, mobil kampanyalardan faydalanmak için mobil app ile giriş yapıp crm sistemine bu bilginin aktarılması.
        /// </remarks>
        /// <returns>Contact dto</returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(UpdateMobileAppRequest), typeof(UpdateMobileAppRequestExamples))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(UpdateMobileAppResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<UpdateMobileAppResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/customer/update-mobileapp")]
        public async Task<IHttpActionResult> UpdateMobileAppAsync(UpdateMobileAppRequest request)
        {
            //await logService.LogSave(LogEventEnum.DbInfo, "Request", nameof(SaveCustomerAsync), CompanyEnum.KD, LogTypeEnum.Request, request);

            var resp = new Response<SaveCustomerResponse>();
            var mappingModel = mapper.Map<SaveCustomerRequest, SaveCustomerRequestDto>(null);
            var response = await contactService.SaveCustomerAsync(mappingModel);
            var mappingResponse = mapper.Map<Response<SaveCustomerResponseDto>, Response<SaveCustomerResponse>>(response);

            //await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(SaveCustomerAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);


            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

    }
}
