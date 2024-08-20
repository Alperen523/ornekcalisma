using AutoMapper;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Examples.Request.Contact;
using UzmanCrm.CrmService.WebAPI.Examples.Response.Contact;
using UzmanCrm.CrmService.WebAPI.Models.Contact;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    // GET: Contact
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "3")] // todo: deployda açılacak
    public class ContactController : ApiController
    {

        private readonly IMapper mapper;
        private readonly ILoginService loginService;
        private readonly ILogService logService;
        private readonly IContactService contactService;
        private readonly IPhoneService phoneService;


        public ContactController(IMapper mapper, ILoginService loginService, ILogService logService, IContactService contactService, IPhoneService phoneService)
        {
            this.mapper = mapper;
            this.loginService = loginService;
            this.logService = logService;
            this.contactService = contactService;
            this.phoneService = phoneService;
        }


        /// <summary>
        /// Search customer
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>
        /// Bu metot,müşteri aramak için kullanılır. 
        /// <br/>Parametrelerden en az bir tanesi doldurulmalıdır. 
        /// <br/>MobilePhone alanı doldurulur ise ilgili eşleşen telefonlar liste olarak getirilir.
        /// <br/>MobilePhone ve EmailAddress alanları doldurulur ise ve ile birbirine bağlanır eşleşen şartlara denk gelen datalar listelenir.
        /// </remarks>
        /// <returns>Contact dto</returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(GetCustomerRequest), typeof(GetCustomerRequestExamples))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(GetCustomerResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<List<GetCustomerResponse>>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/contact/search-customer")]
        public async Task<IHttpActionResult> GetCustomerAsync(GetCustomerRequest request)
        {
            var mappingModel = mapper.Map<GetCustomerRequest, GetCustomerRequestDto>(request);
            var response = await contactService.SearchCustomerAsync(mappingModel);

            var mappingResponse = mapper.Map<Response<List<CustomerDto>>, Response<List<GetCustomerResponse>>>(response);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        /// Save customer ecom
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
        [Route("api/contact/save-customer-ecom")]
        public async Task<IHttpActionResult> SaveCustomerEcomAsync(SaveCustomerRequest request)
        {
            await logService.LogSave(LogEventEnum.DbInfo, "Request", nameof(SaveCustomerEcomAsync), CompanyEnum.KD, LogTypeEnum.Request, request);

            var resp = new Response<SaveCustomerResponse>();
            var mappingModel = mapper.Map<SaveCustomerRequest, SaveCustomerRequestDto>(request);
            var response = await contactService.SaveCustomerAsync(mappingModel);
            var mappingResponse = mapper.Map<Response<SaveCustomerResponseDto>, Response<SaveCustomerResponse>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(SaveCustomerEcomAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!string.IsNullOrEmpty(request.MobilePhone) & (bool)request.KvkkPermit & response.Success)
            {
                var deleteContactPhoneControlResult = await phoneService.DeleteContactPhoneControl(new DeletePhoneRequestDto { Gsm = request.MobilePhone, ErpId = "" });
                await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(deleteContactPhoneControlResult), CompanyEnum.KD, LogTypeEnum.Response, deleteContactPhoneControlResult);
            }

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }


        /// <summary>
        ///  Get customer by ecomid
        /// </summary>
        /// <param name="EcomId">Erp sistemindeki benzersiz müşteri Id bilgisidir</param>
        /// <param name="ChannelId">
        /// Eticaret kanal bilgisi. 
        /// <br/>Vakko (V) = 2890, Vakkorama (R) = 2891, WCollection (W) = 2893
        /// </param>
        /// <remarks>
        ///  Ecom Müşteri getirme metodu. EcomId kullanılarak eşleşen müşteriler aktif veya pasif kayıt olarak getiren metot. 
        /// </remarks>
        /// <returns>EndorsementGetResponse</returns>
        ///  
        [HttpGet]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(GetCustomerByEcomIdResponseExamples))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<GetCustomerByEcomIdResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/contact/get-customer-by-ecomid")]
        public async Task<IHttpActionResult> GetCustomerByEcomIdAsync([Required] string EcomId, [Required] EcomChannelTypeEnum ChannelId)
        {
            var resp = new Response<GetCustomerByEcomIdResponse>();

            var response = await contactService.GetCustomerByEcomIdAsync(EcomId, ChannelId);
            var mappingResponse = mapper.Map<Response<GetCustomerByEcomIdResponseDto>, Response<GetCustomerByEcomIdResponse>>(response);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            switch (ChannelId)
            {
                case EcomChannelTypeEnum.V:
                    mappingResponse.Data.CardNo = response?.Data?.CardNo;
                    mappingResponse.Data.CardType = response?.Data?.CardType;
                    break;
                case EcomChannelTypeEnum.R:
                    mappingResponse.Data.CardNo = response?.Data?.CardNo;
                    mappingResponse.Data.CardType = response?.Data?.CardType;
                    break;
                case EcomChannelTypeEnum.W:
                    mappingResponse.Data.CardNo = response?.Data?.CardNoWcol;
                    mappingResponse.Data.CardType = response?.Data?.CardTypeWcol;
                    break;
                default:
                    break;
            }

            return Ok(mappingResponse);
        }


        /// <summary>
        /// Save customer ecom
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
        [Route("api/contact/save-customer")]
        public async Task<IHttpActionResult> SaveCustomerAsync(SaveCustomerRequest request)
        {
            await logService.LogSave(LogEventEnum.DbInfo, "Request", nameof(SaveCustomerEcomAsync), CompanyEnum.KD, LogTypeEnum.Request, request);

            var resp = new Response<SaveCustomerResponse>();
            var mappingModel = mapper.Map<SaveCustomerRequest, SaveCustomerRequestDto>(request);
            var response = await contactService.SaveCustomerAsync(mappingModel);
            var mappingResponse = mapper.Map<Response<SaveCustomerResponseDto>, Response<SaveCustomerResponse>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(SaveCustomerEcomAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /*
            {
              "FirstName": "alper suat",
              "LastName": "tekin",
              "MobilePhone": "",
              "BirthDate": "",
              "EmailAddress": "",
              "TaxOfficeNo": "",
              "CustomerNo": ""
            }
        */

    }
}