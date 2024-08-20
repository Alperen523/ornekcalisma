using AutoMapper;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Examples.Request.CardClassSegment;
using UzmanCrm.CrmService.WebAPI.Examples.Request.CardExceptionDiscount;
using UzmanCrm.CrmService.WebAPI.Examples.Request.LoyaltyCard;
using UzmanCrm.CrmService.WebAPI.Examples.Response.CardClassSegment;
using UzmanCrm.CrmService.WebAPI.Examples.Response.CardExceptionDiscount;
using UzmanCrm.CrmService.WebAPI.Examples.Response.CustomerGroup;
using UzmanCrm.CrmService.WebAPI.Examples.Response.LoyaltyCard;
using UzmanCrm.CrmService.WebAPI.Models.CardClassSegment;
using UzmanCrm.CrmService.WebAPI.Models.CardExceptionDiscount;
using UzmanCrm.CrmService.WebAPI.Models.CustomerGroup;
using UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard;

namespace UzmanCrm.CrmService.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "3")] // TODO: Deployda açılacak
    public class LoyaltyController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ILogService logService;
        private readonly ILoyaltyCardService loyaltyCardService;
        private readonly ICardClassSegmentService cardClassSegmentService;
        private readonly ICustomerGroupService customerGroupService;
        private readonly ICardExceptionDiscountService cardExceptionDiscountService;
        private readonly IContactService contactService;

        public LoyaltyController(
            IMapper mapper,
            ILogService logService,
            ICardClassSegmentService cardClassSegmentService,
            ILoyaltyCardService loyaltyCardService,
            ICustomerGroupService customerGroupService,
            ICardExceptionDiscountService cardExceptionDiscountService,
            IContactService contactService)
        {
            this.mapper = mapper;
            this.logService = logService;
            this.loyaltyCardService = loyaltyCardService;
            this.customerGroupService = customerGroupService;
            this.cardClassSegmentService = cardClassSegmentService;
            this.cardExceptionDiscountService = cardExceptionDiscountService;
            this.contactService = contactService;
        }

        /// <summary>
        ///     Save Loyalty Card
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///     Bu metot, müşteri sadakat kartı oluşturmak için kullanılır.
        /// </remarks>
        /// <returns>
        ///     LoyaltyCard dto
        /// </returns>
        [HttpPost]
        [Route("api/loyalty/save-loyalty-card")]
        [SwaggerRequestExample(typeof(LoyaltyCardRequest), typeof(LoyaltyCardRequestExample))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(LoyaltyCardSaveResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<LoyaltyCardSaveResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        public async Task<IHttpActionResult> SaveLoyaltyCardAsync(LoyaltyCardRequest request)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Request", nameof(SaveLoyaltyCardAsync), CompanyEnum.KD, LogTypeEnum.Request, request);

            var mappingModel = mapper.Map<LoyaltyCardRequest, LoyaltyCardRequestDto>(request);

            // Kart İstisna tablosu Erp Id bilgisi ile sorgulanacak, Kayıt aktif mi?,Kayıt güncel tarih başlangıç ve bitiş tarihi aralığı içinde mi?,İndirim Onay durumu :Onaylı kayıtlar mı?
            var cardExceptionDiscountRes = await cardExceptionDiscountService.GetCardExceptionDiscountDetailByErpIdAsync(mappingModel.ErpId);

            var response = await loyaltyCardService.LoyaltyCardSaveAsync(mappingModel, cardExceptionDiscountRes);
            var mappingResponse = mapper.Map<Response<LoyaltyCardSaveResponseDto>, Response<LoyaltyCardSaveResponse>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(SaveLoyaltyCardAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        ///     Save Card Class Segment
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///     Bu metot, kart sınıfı segmenti oluşturmak için kullanılır
        /// </remarks>
        /// <returns>
        ///     CardClassSegment dto
        /// </returns>
        [HttpPost]
        [Route("api/loyalty/save-card-class-segment")]
        [SwaggerRequestExample(typeof(CardClassSegmentRequest), typeof(CardClassSegmentRequestExample))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(CardClassSegmentSaveResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<CardClassSegmentSaveResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        public async Task<IHttpActionResult> SaveCardClassSegment(CardClassSegmentRequest request)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Request", nameof(SaveCardClassSegment), CompanyEnum.KD, LogTypeEnum.Request, request);

            var mappingModel = mapper.Map<CardClassSegmentRequest, CardClassSegmentRequestDto>(request);
            var response = await cardClassSegmentService.CardClassSegmentSaveAsync(mappingModel);
            var mappingResponse = mapper.Map<Response<CardClassSegmentSaveResponseDto>, Response<CardClassSegmentSaveResponse>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(SaveCardClassSegment), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        ///     Save Card Exception Discount
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///     Bu metot, müşteri için oluşturulan kart bilgisi kullanılarak istisna indirim tanımlanması oluşturmak için kullanılır
        /// </remarks>
        /// <returns>
        ///     CardExceptionDiscount Dto
        /// </returns>
        [HttpPost]
        [Route("api/loyalty/save-card-exception-discount")]
        [SwaggerRequestExample(typeof(CardExceptionDiscountRequest), typeof(CardExceptionDiscountRequestExample))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(CardExceptionDiscountSaveResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<CardExceptionDiscountSaveResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        public async Task<IHttpActionResult> SaveCardExceptionDiscount(CardExceptionDiscountRequest request)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Request", nameof(SaveCardExceptionDiscount), CompanyEnum.KD, LogTypeEnum.Request, request);

            var mappingModel = mapper.Map<CardExceptionDiscountRequest, CardExceptionDiscountRequestDto>(request);
            var response = await cardExceptionDiscountService.CardExceptionDiscountSaveAsync(mappingModel);
            var mappingResponse = mapper.Map<Response<CardExceptionDiscountSaveResponseDto>, Response<CardExceptionDiscountSaveResponse>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(SaveCardExceptionDiscount), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        ///     Get Loyalty Card By ErpId Or CardNo
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///     Bu metot, müşterilerin sadakat kartı bilgilerini çekmek için kullanılır.
        /// </remarks>
        /// <returns>Loyalty Card</returns>
        [HttpPost]
        [Route("api/loyalty/get-loyalty-card")]
        [SwaggerRequestExample(typeof(LoyaltyCardGetRequest), typeof(LoyaltyCardGetRequestExample))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(LoyaltyCardGetResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<LoyaltyCardGetResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Type = typeof(Response<object>))]
        [SwaggerResponse(System.Net.HttpStatusCode.InternalServerError, Type = typeof(Response<object>))]
        public async Task<IHttpActionResult> GetLoyaltyCardByErpIdOrCardNoAsync(LoyaltyCardGetRequest request)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Request", nameof(GetLoyaltyCardByErpIdOrCardNoAsync), CompanyEnum.KD, LogTypeEnum.Request, request);
            var response = new Response<LoyaltyCardDto>();

            // TODO: Prod geçişte açılacak. 25-09-2023 tarihinde vakko talebi üzerinde kapatıldı
            /*var customerType = "";
            var contactName = "";
            var resContact = await contactService.GetContactItemByErpIdAsync(request.ErpId);
            if (resContact.Success)
            {
                customerType = resContact.Data.uzm_customertype;
                contactName = resContact.Data.FullName;
            }

            if (customerType == "P")
                response = await loyaltyCardService.PersonelDiscountGetAsync(contactName, request.ErpId);
            else
            {*/
            var mappingModel = mapper.Map<LoyaltyCardGetRequest, LoyaltyCardGetRequestDto>(request);
            response = await loyaltyCardService.LoyaltyCardByErpIdOrCardNoGetAsync(mappingModel);
            //}
            var mappingResponse = mapper.Map<Response<LoyaltyCardDto>, Response<LoyaltyCardGetResponse>>(response);

            if (mappingResponse.Success)
            {
                switch (request.ChannelType)
                {
                    case CardTypeEnum.V:
                        mappingResponse.Data.ValidDiscountRate = response?.Data?.uzm_amountforuppersegmentvakko == null ? mappingResponse.Data.ValidDiscountRate : (double)response?.Data?.uzm_validdiscountratevakko;
                        break;
                    case CardTypeEnum.R:
                        mappingResponse.Data.ValidDiscountRate = response?.Data?.uzm_amountforuppersegmentvr == null ? mappingResponse.Data.ValidDiscountRate : (double)response?.Data?.uzm_validdiscountratevr;
                        break;
                    case CardTypeEnum.W:
                        mappingResponse.Data.ValidDiscountRate = response?.Data?.uzm_amountforuppersegmentwcol == null ? mappingResponse.Data.ValidDiscountRate : (double)response?.Data?.uzm_validdiscountratewcol;
                        break;
                    default:
                        break;
                }
            }

            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Response", nameof(GetLoyaltyCardByErpIdOrCardNoAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        ///     Get Customer Group List
        /// </summary>
        /// <param name="CustomerGroupCode">Müşteri Grup Kodu bilgisidir</param>
        /// <remarks>
        ///     Bu metot, müşteri grubu bilgilerini çekmek için kullanılır.
        ///     <br/>Müşteri grup kodu parametresi dolu ise aynı kodda olan veriyi getirir. Parametre boşsa tüm müşteri gruplarını listeler.
        /// </remarks>
        /// <returns>Customer Group List</returns>
        [HttpGet]
        [Route("api/loyalty/customer-group-list")]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(CustomerGroupGetResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<List<CustomerGroupGetResponse>>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<List<object>>))]
        public async Task<IHttpActionResult> CustomerGroupList(string CustomerGroupCode = null)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Request", nameof(CustomerGroupList), CompanyEnum.KD, LogTypeEnum.Request, CustomerGroupCode);

            var response = await customerGroupService.GetCustomerGroupListAsync(CustomerGroupCode);
            var mappingResponse = mapper.Map<Response<List<CustomerGroupGetDto>>, Response<List<CustomerGroupGetResponse>>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Response", nameof(CustomerGroupList), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        ///     Get Card Class Segment
        /// </summary>
        /// <param name="SegmentName">Segment Adı bilgisidir</param>
        /// <remarks>
        ///     Bu metot, kart sınıfı segmenti bilgilerini çekmek için kullanılır.
        ///     <br/>Segment adı parametresi dolu ise aynı adda olan veriyi getirir. Parametre boşsa tüm kart sınıfı segmentlerini listeler.
        /// </remarks>
        /// <returns>CardClassSegment</returns>
        [HttpGet]
        [Route("api/loyalty/card-class-segment-list")]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(CardClassSegmentGetResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<List<CardClassSegmentGetResponse>>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<List<object>>))]
        public async Task<IHttpActionResult> CardClassSegmentList(string SegmentName = null)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Request", nameof(CardClassSegmentList), CompanyEnum.KD, LogTypeEnum.Request, SegmentName);

            var response = await cardClassSegmentService.GetCardClassSegmentListAsync(SegmentName);
            var mappingResponse = mapper.Map<Response<List<CardClassSegmentDto>>, Response<List<CardClassSegmentGetResponse>>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Response", nameof(CardClassSegmentList), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }
    }
}
