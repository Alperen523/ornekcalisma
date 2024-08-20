using AutoMapper;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Examples.Request.Endorsement;
using UzmanCrm.CrmService.WebAPI.Examples.Response.Endorsement;
using UzmanCrm.CrmService.WebAPI.Models.Endorsement;

namespace UzmanCrm.CrmService.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "2")] // TODO: Deployda açılacak
    public class CustomerEndorsementController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ILogService logService;
        private readonly IEndorsementService endorsementService;
        private readonly IContactService contactService;
        private readonly ILoyaltyCardService loyaltyCardService;

        public CustomerEndorsementController(IMapper mapper, ILogService logService, IEndorsementService endorsementService, IContactService contactService, ILoyaltyCardService loyaltyCardService)
        {
            this.mapper = mapper;
            this.logService = logService;
            this.endorsementService = endorsementService;
            this.contactService = contactService;
            this.loyaltyCardService = loyaltyCardService;
        }

        /// <summary>
        /// Customer endorsement save
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        /// Bu metot, müşterilerin ciro bilgilerini kaydetmek için kullanılır. 
        /// <br/>SAP mümkün olan en kısa aralıklarla yeni gelen fatura(ciro) bilgilerini bu metot ile tetiklemelidir.
        /// </remarks>
        /// <returns>Endorsement dto</returns>
        [HttpPost]
        [Route("api/customer-endorsements/save")]
        [SwaggerRequestExample(typeof(EndorsementRequest), typeof(EndorsementRequestExample))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(EndorsementResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<EndorsementSaveResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        public async Task<IHttpActionResult> CustomerEndorsementSaveAsync(EndorsementRequest request)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Request", nameof(CustomerEndorsementSaveAsync), CompanyEnum.KD, LogTypeEnum.Request, request);

            var mappingModel = mapper.Map<EndorsementRequest, EndorsementRequestDto>(request);
            var response = await endorsementService.EndorsementSaveAsync(mappingModel);
            var mappingResponse = mapper.Map<Response<EndorsementSaveResponseDto>, Response<EndorsementSaveResponse>>(response);

            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(CustomerEndorsementSaveAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }

        /// <summary>
        ///     Get Endorsement
        /// </summary>
        /// <param name="ErpId">Erp sistemindeki benzersiz müşteri Id bilgisidir</param>
        /// <param name="ChannelType">
        ///     İlgili müşterinin hangi marka için ciro-indirim bilgisi istenildiği bilgisidir<br/>
        ///     Unknown = 0,<br/>
        ///     V = 1, // Vakko <br/>
        ///     R = 2, // Vakkorama <br/>
        ///     W = 3 // WCollection
        ///     P = 4 // Personel
        /// </param>
        /// <remarks>
        ///     Bu metot, müşteri ciro güncel durum bilgilerini çekmek için kullanılır.
        /// </remarks>
        /// <returns>EndorsementGetResponse</returns>
        [HttpGet]
        [Route("api/customer-endorsements")]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(EndorsementGetResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<EndorsementGetResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [SwaggerResponse(System.Net.HttpStatusCode.NotFound, Type = typeof(Response<object>))]
        [SwaggerResponse(System.Net.HttpStatusCode.InternalServerError, Type = typeof(Response<object>))]
        public async Task<IHttpActionResult> CustomerEndorsementGetAsync(string ErpId, CardTypeEnum ChannelType)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Request", nameof(CustomerEndorsementGetAsync), CompanyEnum.KD, LogTypeEnum.Request, ErpId);
            var response = new Response<LoyaltyCardDto>();

            // TODO: Prod geçişte açılacak. 25-09-2023 tarihinde vakko talebi üzerinde kapatıldı
            /*var customerType = "";
            var contactName = "";
            var resContact = await contactService.GetContactItemByErpIdAsync(ErpId);
            if (resContact.Success)
            {
                customerType = resContact.Data.uzm_customertype;
                contactName = resContact.Data.FullName;
            }

            if (customerType == "P")
                response = await loyaltyCardService.PersonelDiscountGetAsync(contactName, ErpId);
            else*/
            response = await endorsementService.EndorsementGetAsync(ErpId, ChannelType);

            var mappingResponse = mapper.Map<Response<LoyaltyCardDto>, Response<EndorsementGetResponse>>(response);
            if (mappingResponse.Success)
            {
                /*
                // Müşteri tipi P ise channle type dan bağımsız indirim oran sonucunu döner
                // TODO: Prod geçişte açılacak. 25-09-2023 tarihinde vakko talebi üzerinde kapatıldı
                /*if (customerType == "P")
                    mappingResponse.Data.DiscountPercent = response?.Data?.uzm_validdiscountratevakko == null ? mappingResponse.Data.DiscountPercent : (double)response?.Data?.uzm_validdiscountratevakko;
                else
                {*/
                switch (ChannelType)
                {
                    case CardTypeEnum.V:
                        mappingResponse.Data.AmountForUpperSegment = response?.Data?.uzm_amountforuppersegmentvakko == null ? mappingResponse.Data.AmountForUpperSegment : (double)response?.Data?.uzm_amountforuppersegmentvakko;
                        mappingResponse.Data.UpperSegmentDiscountPercent = response?.Data?.uzm_uppersegmentdiscountpercentvakko == null ? mappingResponse.Data.UpperSegmentDiscountPercent : (double)response?.Data?.uzm_uppersegmentdiscountpercentvakko;
                        mappingResponse.Data.DiscountPercent = response?.Data?.uzm_validdiscountratevakko == null ? mappingResponse.Data.DiscountPercent : (double)response?.Data?.uzm_validdiscountratevakko;
                        break;
                    case CardTypeEnum.R:
                        mappingResponse.Data.AmountForUpperSegment = response?.Data?.uzm_amountforuppersegmentvr == null ? mappingResponse.Data.AmountForUpperSegment : (double)response?.Data?.uzm_amountforuppersegmentvr;
                        mappingResponse.Data.UpperSegmentDiscountPercent = response?.Data?.uzm_uppersegmentdiscountpercentvr == null ? mappingResponse.Data.UpperSegmentDiscountPercent : (double)response?.Data?.uzm_uppersegmentdiscountpercentvr;
                        mappingResponse.Data.DiscountPercent = response?.Data?.uzm_validdiscountratevr == null ? mappingResponse.Data.DiscountPercent : (double)response?.Data?.uzm_validdiscountratevr;
                        break;
                    case CardTypeEnum.W:
                        mappingResponse.Data.AmountForUpperSegment = response?.Data?.uzm_amountforuppersegmentwcol == null ? mappingResponse.Data.AmountForUpperSegment : (double)response?.Data?.uzm_amountforuppersegmentwcol;
                        mappingResponse.Data.UpperSegmentDiscountPercent = response?.Data?.uzm_uppersegmentdiscountpercentwcol == null ? mappingResponse.Data.UpperSegmentDiscountPercent : (double)response?.Data?.uzm_uppersegmentdiscountpercentwcol;
                        mappingResponse.Data.DiscountPercent = response?.Data?.uzm_validdiscountratewcol == null ? mappingResponse.Data.DiscountPercent : (double)response?.Data?.uzm_validdiscountratewcol;
                        break;
                    default:
                        break;
                }
                //}
                mappingResponse.Data.UpperSegmentDiscountPercent = mappingResponse.Data.DiscountPercent > mappingResponse.Data.UpperSegmentDiscountPercent ? 0 : mappingResponse.Data.UpperSegmentDiscountPercent;
                mappingResponse.Data.AmountForUpperSegment = mappingResponse.Data.DiscountPercent > mappingResponse.Data.UpperSegmentDiscountPercent ? 0 : mappingResponse.Data.AmountForUpperSegment;
            }

            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog, "Response", nameof(CustomerEndorsementGetAsync), CompanyEnum.KD, LogTypeEnum.Response, mappingResponse);

            if (!mappingResponse.Success)
                return Content(mappingResponse.Error.StatusCode, mappingResponse);

            return Ok(mappingResponse);
        }
    }
}
