using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.LoyaltyCard;

namespace UzmanCrm.CrmService.Application.Service.LoyaltyCardService
{
    public class LoyaltyCardService : ILoyaltyCardService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly ICRMService crmService;
        private readonly ICardTypeService cardTypeService;
        private readonly IContactService contactService;
        private readonly IWageScaleService wageScaleService;

        public LoyaltyCardService(IMapper mapper,
            ICRMService crmService,
            IDapperService dapperService,
            ILogService logService,
            ICardTypeService cardTypeService,
            IContactService contactService,
            IWageScaleService wageScaleService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
            this.crmService = crmService;
            this.cardTypeService = cardTypeService;
            this.contactService = contactService;
            this.wageScaleService = wageScaleService;
        }

        /// <summary>
        /// Get LoyaltyCard for CRM
        /// ErpId veya Kart Numarası bilgilerini kullanarak sadakat kart sorgulama
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardDto>> LoyaltyCardByErpIdOrCardNoGetAsync(LoyaltyCardGetRequestDto requestDto)
        {
            var model = new Response<LoyaltyCardDto>();

            var getLoyaltyCardResponse = await GetLoyaltyCardByErpIdOrCardNoAsync(requestDto);
            if (getLoyaltyCardResponse.Success)
            {
                model.Success = true;
                model.Message = CommonStaticConsts.Message.Success;
                model.Data = getLoyaltyCardResponse.Data;
            }
            else
            {
                model.Data = null;
                model.Success = false;
                model.Error = getLoyaltyCardResponse.Error != null ? getLoyaltyCardResponse.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.LoyaltyCardGetError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.LoyaltyCardStaticConsts.LC0001 });
                model.Message = getLoyaltyCardResponse.Message;

                return model;
            }

            return model;
        }

        /// <summary>
        /// ErpId veya Kart Numarası bilgilerini kullanarak sadakat kart sorgulama
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<Response<LoyaltyCardDto>> GetLoyaltyCardByErpIdOrCardNoAsync(LoyaltyCardGetRequestDto requestDto)
        {
            var cardTypeList = GeneralHelper.GetCardTypeFilter(requestDto.ChannelType);

            var whereStatement = "";
            if (requestDto.CardNo.IsNotNullAndEmpty() && requestDto.ErpId.IsNullOrEmpty())
                whereStatement = $"AND lc.uzm_cardnumber = '{requestDto.CardNo}'";
            else if (requestDto.CardNo.IsNullOrEmpty() && requestDto.ErpId.IsNotNullAndEmpty())
                whereStatement = $"AND lc.uzm_erpId = '{requestDto.ErpId}'";
            else if (requestDto.CardNo.IsNotNullAndEmpty() && requestDto.ErpId.IsNotNullAndEmpty())
                whereStatement = $"AND (lc.uzm_cardnumber = '{requestDto.CardNo}' OR lc.uzm_erpId='{requestDto.ErpId}')";

            var queryLoyaltyCard = @$"SELECT
lc.[uzm_loyaltycardId],
lc.[uzm_cardnumber],
lc.[uzm_cardtypedefinitionidName],
lc.uzm_contactidName,
lc.uzm_erpId,
lc.uzm_statuscode,
lc.uzm_validdiscountratevakko,
lc.uzm_validdiscountratevr,
lc.uzm_validdiscountratewcol,
lc.uzm_periodendorsement,
lc.uzm_validendorsement,
lc.uzm_turnoverendorsement,
CASE
    WHEN lc.uzm_validdiscountratevakko > lc.uzm_uppersegmentdiscountpercentvakko THEN 0
    ELSE lc.uzm_amountforuppersegmentvakko
    END AS uzm_amountforuppersegmentvakko,
CASE
    WHEN lc.uzm_validdiscountratevr > lc.uzm_uppersegmentdiscountpercentvr THEN 0
    ELSE lc.uzm_amountforuppersegmentvr
    END AS uzm_amountforuppersegmentvr,
CASE
    WHEN lc.uzm_validdiscountratewcol > lc.uzm_uppersegmentdiscountpercentwcol THEN 0
    ELSE lc.uzm_amountforuppersegmentwcol
    END AS uzm_amountforuppersegmentwcol,
CASE
    WHEN lc.uzm_validdiscountratevakko > lc.uzm_uppersegmentdiscountpercentvakko THEN 0
    ELSE lc.uzm_uppersegmentdiscountpercentvakko
    END AS uzm_uppersegmentdiscountpercentvakko,
CASE
    WHEN lc.uzm_validdiscountratevr > lc.uzm_uppersegmentdiscountpercentvr THEN 0
    ELSE lc.uzm_uppersegmentdiscountpercentvr
    END AS uzm_uppersegmentdiscountpercentvr,
CASE
    WHEN lc.uzm_validdiscountratewcol > lc.uzm_uppersegmentdiscountpercentwcol THEN 0
    ELSE lc.uzm_uppersegmentdiscountpercentwcol
    END AS uzm_uppersegmentdiscountpercentwcol,
CASE WHEN lc.statecode=0 THEN 'A' ELSE 'P' END AS State,
DATEADD(HOUR, 3, lc.ModifiedOn) AS modifiedon,
DATEADD(HOUR, 3, lc.CreatedOn) AS createdon,
bu.Name AS storename
FROM [uzm_loyaltycard] lc WITH(NOLOCK)
left JOIN BusinessUnit bu WITH(NOLOCK) on bu.uzm_accountcode = lc.uzm_storecode
WHERE lc.statecode=0 AND lc.uzm_statuscode=1 and lc.uzm_cardtypedefinitionidName {cardTypeList} {whereStatement}";
            var res = await dapperService.GetItemParam<object, LoyaltyCardDto>(queryLoyaltyCard, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
            return res;
        }

        /// <summary>
        /// Müşteri Sadakat Kartı güncellemesi ortak kullanım metodu
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardUpdateResponseDto>> LoyaltyCardUpdateAsync(LoyaltyCardUpdateDto requestDto)
        {
            var responseModel = new Response<LoyaltyCardUpdateResponseDto>();
            try
            {
                var entityModel = mapper.Map<LoyaltyCard>(requestDto);

                var result = crmService.Save<LoyaltyCard>(entityModel, "uzm_loyaltycard", "uzm_loyaltycard", Common.Enums.CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(LoyaltyCardUpdateAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardUpdateResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.CardExceptionDiscountSaveError + ex.ToString(), "")));
            }

            return responseModel;
        }

        /// <summary>
        /// Loyalty card save for CRM
        /// Api ile gelen isteğe göre kart numarası oluşturarak ve gerekli ilişkileri kurarak sadakat kartı oluşturma
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardSaveResponseDto>> LoyaltyCardSaveAsync(LoyaltyCardRequestDto requestDto, Response<CardExceptionDiscountDto> cardExceptionDiscountRes)
        {
            var loyaltyCardInfoOnContactReq = new LoyaltyCardInfoOnContactRequest();
            requestDto.CardStatusCodeType = LoyaltyCardStatusCodeType.InUse;
            var entityModel = new LoyaltyCard();
            var responseModel = new Response<LoyaltyCardSaveResponseDto>();
            var modelDto = mapper.Map<LoyaltyCardDto>(requestDto);
            var cardNumbertryCount = 0;
            var cardNo = "";

            try
            {
            cardnumbertag:
                var cardNumberResult = await CreateCardNumber(modelDto.uzm_cardtypedefinitionid.ToString());
                if (cardNumberResult.Success)
                {
                    cardNo = (string)cardNumberResult.Data;
                }
                else
                {
                    return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest, cardNumberResult.Message, ErrorStaticConsts.LoyaltyCardStaticConsts.LC0004)));
                }
                cardNumbertryCount++;
                var resService = await contactService.GetContactItemByErpIdAsync(requestDto.ErpId);
                if (resService.Success)
                {
                    if (resService?.Data?.uzm_customertype == "M")
                    {
                        entityModel = mapper.Map<LoyaltyCard>(resService);
                        entityModel.uzm_cardnumber = cardNo;
                        entityModel.uzm_subcardnumber = cardNo.Substring(cardNo.Length - 12); // Kart numarasının son 12 hanesi
                        entityModel.uzm_cardtypedefinitionid = modelDto.uzm_cardtypedefinitionid;
                        entityModel.uzm_statuscode = (int)requestDto.CardStatusCodeType;
                        entityModel.uzm_storecode = modelDto.uzm_storecode;
                        //uzm_storecode dolu ise businessid (storeid) bilgisini getir.
                        var resServiceStoreId = await GetBusinessUnitIdByStoreCodeAsync(requestDto);
                        if (resServiceStoreId.Success)
                            entityModel.uzm_storeid = resServiceStoreId.Data.uzm_storeid;


                        requestDto.CrmId = entityModel.uzm_contactid;
                        var resServiceCard = await GetLoyaltyCardByCrmIdItemAsync(requestDto);
                        if (resServiceCard.Success)
                        {
                            // kayıtlı loyalty card varsa müşteri kartı üzerindeki lookup a set edilir
                            loyaltyCardInfoOnContactReq.contactid = (Guid)entityModel.uzm_contactid;
                            loyaltyCardInfoOnContactReq.uzm_loyaltycardid = (Guid)resServiceCard?.Data?.uzm_loyaltycardid;
                            loyaltyCardInfoOnContactReq.uzm_loyaltycardno = resServiceCard?.Data?.uzm_cardnumber;// cardNo;
                            loyaltyCardInfoOnContactReq.uzm_loyaltycardtype = (CardTypeEnum)Enum.Parse(typeof(CardTypeEnum), resServiceCard?.Data?.uzm_cardtypedefinitionidname);
                            var resp = await UpdateLoyaltyCardInfoOnContactAsync(loyaltyCardInfoOnContactReq);
                            if (!resp.Success)
                            {
                                return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                      CommonStaticConsts.Message.LoyaltyCardInfoOnContactUpdateError + resp.Message.ToString(), "")));
                            }
                            return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardSaveResponseDto>(
                                new ErrorModel(System.Net.HttpStatusCode.BadRequest, CommonStaticConsts.Message.LoyaltyCardAvailable, ErrorStaticConsts.LoyaltyCardStaticConsts.LC0002)));
                        }
                    }
                    else
                    {
                        return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardSaveResponseDto>(
                                new ErrorModel(System.Net.HttpStatusCode.BadRequest, CommonStaticConsts.Message.SaveLoyaltyCardCustomerTypeError, ErrorStaticConsts.LoyaltyCardStaticConsts.LC0005)));
                    }
                }
                else
                {
                    return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardSaveResponseDto>(
                         new ErrorModel(System.Net.HttpStatusCode.BadRequest, CommonStaticConsts.Message.CustomerErpNotFound, ErrorStaticConsts.LoyaltyCardStaticConsts.LC0003)));
                }
                entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);

                var wagescaleListRes = await wageScaleService.ValidWageScaleGetList();
                if (wagescaleListRes.Success)
                {
                    var cardTypeIdVakko = new Guid("DC6B7D06-3E33-ED11-915C-00505685232B"); // Vakko Kart Tipi
                    var minWageScaleVakko = wageScaleService.GetBiggerThanZeroDiscountMinWagescaleByCardType(wagescaleListRes.Data, cardTypeIdVakko);
                    if (minWageScaleVakko.Success)
                    {
                        entityModel.uzm_validdiscountratevakko = 0.0;
                        entityModel.uzm_uppersegmentdiscountpercentvakko = minWageScaleVakko.Data.uzm_discountrate;
                        entityModel.uzm_amountforuppersegmentvakko = minWageScaleVakko.Data.uzm_rangestart;
                    }

                    var cardTypeIdVr = new Guid("7ECA170E-3E33-ED11-915C-00505685232B"); // Vakkorama Kart Tipi
                    var minWageScaleVr = wageScaleService.GetBiggerThanZeroDiscountMinWagescaleByCardType(wagescaleListRes.Data, cardTypeIdVr);
                    if (minWageScaleVr.Success)
                    {
                        entityModel.uzm_validdiscountratevr = 0.0;
                        entityModel.uzm_uppersegmentdiscountpercentvr = minWageScaleVr.Data.uzm_discountrate;
                        entityModel.uzm_amountforuppersegmentvr = minWageScaleVr.Data.uzm_rangestart;
                    }

                    // WCollection'da her marka kendi baremine göre değerlendirildiği için loyalty card'ın kart tipi bilgisine göre barem sorgusu yazılması gerekir. Marka bazlı Geçerli İndirim Oranı, üst bareme kalan tutar, üst barem indirim oranı alanları o markanın barem kaydındaki alanlar ile doldurulur.
                    var cardTypeIdWcol = Guid.Empty;
                    switch (requestDto.CardType)
                    {
                        case CardTypeEnum.V:
                            cardTypeIdWcol = new Guid("DC6B7D06-3E33-ED11-915C-00505685232B"); // Vakko Kart Tipi
                            break;
                        case CardTypeEnum.R:
                            cardTypeIdWcol = new Guid("7ECA170E-3E33-ED11-915C-00505685232B"); // Vakkorama Kart Tipi
                            break;
                        case CardTypeEnum.W:
                            cardTypeIdWcol = new Guid("3CDAC57E-CC29-ED11-915B-00505685232B"); // WCollection Kart Tipi
                            break;
                        default:
                            break;
                    }
                    var minWageScaleWcol = wageScaleService.GetBiggerThanZeroDiscountMinWagescaleByCardType(wagescaleListRes.Data, cardTypeIdWcol);
                    if (minWageScaleWcol.Success)
                    {
                        entityModel.uzm_validdiscountratewcol = 0.0;
                        entityModel.uzm_uppersegmentdiscountpercentwcol = minWageScaleWcol.Data.uzm_discountrate;
                        entityModel.uzm_amountforuppersegmentwcol = minWageScaleWcol.Data.uzm_rangestart;
                    }
                }
                
                // İstisna kontrolü
                if (cardExceptionDiscountRes.Success)
                {
                    entityModel.uzm_carddiscountid = cardExceptionDiscountRes.Data.uzm_carddiscountId;

                    var cardExceptionDiscountRate = 0.0; // Kart İstisna Tanımı İndirim Oranı
                    cardExceptionDiscountRate = (double)cardExceptionDiscountRes.Data.uzm_discountrate;
                    // İleriye dönük olarak entitymodel ile kıyaslayarak setleme işlemi yapılıyor
                    if (cardExceptionDiscountRate > entityModel.uzm_validdiscountratevakko)
                        entityModel.uzm_validdiscountratevakko = cardExceptionDiscountRate;
                    if (cardExceptionDiscountRate > entityModel.uzm_validdiscountratevr)
                        entityModel.uzm_validdiscountratevr = cardExceptionDiscountRate;
                    if (cardExceptionDiscountRate > entityModel.uzm_validdiscountratewcol)
                        entityModel.uzm_validdiscountratewcol = cardExceptionDiscountRate;
                }

                // Kart tipi W ise V ve R tipi alanları boşalt
                if (requestDto.CardType == CardTypeEnum.W)
                {
                    entityModel.uzm_validdiscountratevakko = null;
                    entityModel.uzm_validdiscountratevr = null;

                    entityModel.uzm_uppersegmentdiscountpercentvakko = null;
                    entityModel.uzm_uppersegmentdiscountpercentvr = null;

                    entityModel.uzm_amountforuppersegmentvakko = null;
                    entityModel.uzm_amountforuppersegmentvr = null;
                }

                // Kart tipi V veya R ise W tipi için alanları boşalt
                if (requestDto.CardType == CardTypeEnum.V || requestDto.CardType == CardTypeEnum.R)
                {
                    entityModel.uzm_validdiscountratewcol = null;
                    entityModel.uzm_uppersegmentdiscountpercentwcol = null;
                    entityModel.uzm_amountforuppersegmentwcol = null;
                }

                var result = crmService.Save<LoyaltyCard>(entityModel, "uzm_loyaltycard", "uzm_loyaltycard", Common.Enums.CompanyEnum.KD);
                // Kart numarası üretme yarım saniye beklenerek 5 kere denenir. 5'inde de içeride kayıtlı bir kart numarası üretilirse hata mesajı olarak dönülür
                if (!result.Success && (result.Message.Contains("Unique") || result.Message.Contains("Duplicate")) && cardNumbertryCount <= 5)
                {
                    await Task.Delay(500);
                    goto cardnumbertag;
                }
                responseModel.Success = result.Success;
                responseModel.Message = "LoyaltyCardSaveResult:" + result.Message;
                responseModel.Error = result.Error;
                if (result.Success)
                {
                    responseModel.Data.Id = result.Data;
                    responseModel.Data.CrmId = entityModel.uzm_contactid;
                    responseModel.Data.CardNo = cardNo;
                    loyaltyCardInfoOnContactReq.contactid = (Guid)entityModel.uzm_contactid;
                    loyaltyCardInfoOnContactReq.uzm_loyaltycardid = result.Data;
                    loyaltyCardInfoOnContactReq.uzm_loyaltycardno = cardNo;
                    loyaltyCardInfoOnContactReq.uzm_loyaltycardtype = requestDto.CardType;
                    var resp = await UpdateLoyaltyCardInfoOnContactAsync(loyaltyCardInfoOnContactReq);
                    if (!resp.Success)
                    {
                        return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
              CommonStaticConsts.Message.LoyaltyCardInfoOnContactUpdateError + resp.Message.ToString(), "")));
                    }
                }
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(LoyaltyCardSaveAsync),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );
                return await Task.FromResult(ResponseHelper.SetSingleError<LoyaltyCardSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.LoyaltyCardSaveError + ex.ToString(), "")));
            }
            return responseModel;
        }

        /// <summary>
        /// Update Loyalty Card Info On Contact Async Müşteri kartındaki loyalty kart alanını güncelleme
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<Response<UpdateResponse>> UpdateLoyaltyCardInfoOnContactAsync(LoyaltyCardInfoOnContactRequest request)
        {
            var responseModel = new Response<UpdateResponse>();

            var updateContactRes = await contactService.UpdateLoyaltyCardInfoOnContactAsync(request);
            responseModel.Success = updateContactRes.Success;
            responseModel.Message = "UpdateContactResult : " + updateContactRes.Message;
            responseModel.Error = updateContactRes.Error;

            return responseModel;
        }

        /// <summary>
        /// contact id bilgisi ile sadakat kart sorgulama
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<Response<LoyaltyCardDto>> GetLoyaltyCardByCrmIdItemAsync(LoyaltyCardRequestDto requestDto)
        {
            var cardTypeList = GeneralHelper.GetCardTypeFilter(requestDto.CardType);

            var query = String.Format($@"
             select lc.uzm_cardnumber, lc.uzm_loyaltycardId, lc.uzm_cardtypedefinitionidname
             from ContactBase c with(nolock) 
             inner join uzm_loyaltycard lc with(nolock)  on lc.uzm_contactid=c.ContactId
             where c.ContactId=@CrmId and c.statecode=0 and lc.statecode=0 and lc.uzm_statuscode=1 and uzm_cardtypedefinitionidName {cardTypeList}");
            var resService = await dapperService.GetItemParam<object, LoyaltyCardDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        /// <summary>
        /// Tüm loyalty kart kayıtları içerisinde en büyük sub card number alanına 1 ekleyerek yeni kart numarası oluşturma
        /// </summary>
        /// <returns></returns>
        private async Task<Response<LoyaltyCardDto>> GetNewLoyaltyCardNumberAsync()
        {
            var query = String.Format($@"
SELECT MAX(CAST(uzm_subcardnumber AS bigint)) + 1 AS uzm_subcardnumber
FROM [KahveDunyasi_MSCRM].[dbo].uzm_loyaltycardBase WITH(NOLOCK)
");
            var resService = await dapperService.GetItemParam<object, LoyaltyCardDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;

        }

        /// <summary>
        /// Aynı kart tipinde ve anı kart tipi kodu ile kayıtlı kart varsa kart numarası en büyük olanı bul, 1 ekleyerek yeni numarasını oluştur
        /// Yoksa Kart Tipi tablosundan 4 haneli kodu alıp 16 haneye tamamlayacak şekilde 1'den başlayan yeni numarayı oluştur
        /// </summary>
        /// <param name="cardTypeId"></param>
        /// <param name="cardTypeName"></param>
        /// <returns></returns>
        private async Task<Response<object>> CreateCardNumber(string cardTypeId)
        {
            var resp = new Response<object>();

            var cardTypeResult = await cardTypeService.GetCardTypeByIdItemAsync(cardTypeId);
            if (cardTypeResult.Success)
            {
                var cardTypeCode = cardTypeResult?.Data?.uzm_code;
                if (cardTypeCode.IsNotNullAndEmpty())
                {
                    resp.Success = true;
                    resp.Message = CommonStaticConsts.Message.Success;
                    var cardNoResult = await GetNewLoyaltyCardNumberAsync();
                    if (cardNoResult.Success && cardNoResult.Data.uzm_subcardnumber.IsNotNullAndEmpty())
                    {
                        var subcardnumber = Convert.ToInt64(cardNoResult.Data.uzm_subcardnumber).ToString("D12");
                        resp.Data = $"{cardTypeCode}{subcardnumber}";
                    }

                    else
                        resp.Data = $"{cardTypeCode}000000000001";
                }
                else
                {
                    resp.Message = $"{cardTypeResult?.Data?.uzm_name} isimli kart tipinin Kart Tipi Kodu alanı veri içermemektedir.";
                }
            }
            else
            {
                resp.Message = CommonStaticConsts.Message.CardTypeNotFound;
            }

            return resp;
        }

        /// <summary>
        /// Kart numarası ve card tipi id bilgileri kullanılarak kullanımda olan sadakat kart kaydını bulma
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="cardTypeId"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardDto>> GetLoyaltyCardByCardNoAndCardTypeAsync(string cardNo, string cardTypeId)
        {
            var query = $@"
SELECT
	uzm_loyaltycardId,
	uzm_cardnumber,
	uzm_cardtypedefinitionid,
	uzm_cardtypedefinitionidName,
	uzm_contactidName,
	uzm_erpId,
	uzm_statuscode,
	uzm_validdiscountratevakko,
    uzm_validdiscountratevr,
    uzm_validdiscountratewcol,
	uzm_periodendorsement,
	uzm_validendorsement,
	uzm_turnoverendorsement,
	uzm_amountforuppersegmentvakko,
	uzm_amountforuppersegmentvr,
	uzm_amountforuppersegmentwcol,
	uzm_uppersegmentdiscountpercentvakko,
	uzm_uppersegmentdiscountpercentvr,
	uzm_uppersegmentdiscountpercentwcol,
	CASE WHEN statecode=0 THEN 'A' ELSE 'P' END AS State,
	DATEADD(HOUR, 3, ModifiedOn) AS modifiedon
FROM [uzm_loyaltycard] WITH(NOLOCK)
WHERE uzm_cardnumber='{cardNo}' AND uzm_cardtypedefinitionid='{cardTypeId}'
AND uzm_statuscode=1 AND statecode=0";
            return await dapperService.GetItemParam<object, LoyaltyCardDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Erp Id bilgisi kullanılarak kullanımda olan sadakat kart kaydını bulma
        /// </summary>
        /// <param name="erpId"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardDto>> GetLoyaltyCardByErpIdAsync(string erpId)
        {
            var query = $@"
SELECT
	uzm_loyaltycardId,
	uzm_cardnumber,
	uzm_cardtypedefinitionid,
	uzm_cardtypedefinitionidName,
	uzm_contactidName,
	uzm_statuscode,
	uzm_validdiscountratevakko,
    uzm_validdiscountratevr,
    uzm_validdiscountratewcol,
	uzm_periodendorsement,
	uzm_validendorsement,
	uzm_turnoverendorsement,
	uzm_amountforuppersegmentvakko,
	uzm_amountforuppersegmentvr,
	uzm_amountforuppersegmentwcol,
	uzm_uppersegmentdiscountpercentvakko,
	uzm_uppersegmentdiscountpercentvr,
	uzm_uppersegmentdiscountpercentwcol,
	CASE WHEN statecode=0 THEN 'A' ELSE 'P' END AS State,
	DATEADD(HOUR, 3, ModifiedOn) AS modifiedon
FROM [uzm_loyaltycard] WITH(NOLOCK)
WHERE uzm_erpId='{erpId}' AND uzm_statuscode=1 AND statecode=0";
            return await dapperService.GetItemParam<object, LoyaltyCardDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Loyalty Card Id bilgisi kullanılarak kullanımda olan sadakat kart kaydını bulma
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardDto>> GetLoyaltyCardByIdAsync(Guid Id)
        {
            var query = $@"
SELECT
	uzm_loyaltycardId,
	uzm_cardnumber,
	uzm_cardtypedefinitionid,
	uzm_cardtypedefinitionidName,
	uzm_contactidName,
	uzm_statuscode,
	uzm_validdiscountratevakko,
    uzm_validdiscountratevr,
    uzm_validdiscountratewcol,
	uzm_periodendorsement,
	uzm_validendorsement,
	uzm_turnoverendorsement,
	uzm_amountforuppersegmentvakko,
	uzm_amountforuppersegmentvr,
	uzm_amountforuppersegmentwcol,
	uzm_uppersegmentdiscountpercentvakko,
	uzm_uppersegmentdiscountpercentvr,
	uzm_uppersegmentdiscountpercentwcol,
	CASE WHEN statecode=0 THEN 'A' ELSE 'P' END AS State,
	DATEADD(HOUR, 3, ModifiedOn) AS modifiedon,
    uzm_carddiscountid,
	uzm_erpId
FROM [uzm_loyaltycard] WITH(NOLOCK)
WHERE uzm_loyaltycardId='{Id}' AND uzm_statuscode=1 AND statecode=0";
            return await dapperService.GetItemParam<object, LoyaltyCardDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Müşteri Sadakat Kartı SQL komutu ile güncelleme
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardUpdateResponseDto>> LoyaltyCardEndorsementFieldsUpdateWithSqlAsync(LoyaltyCardUpdateDto requestDto)
        {
            var query = $@"
BEGIN TRANSACTION
BEGIN TRY
	BEGIN
		declare @lcCount int; set @lcCount=0;
		declare @ceCount int; set @ceCount=0;
		Update [uzm_loyaltycard] set
			uzm_validendorsement={requestDto.ValidEndorsement},
			uzm_periodendorsement={requestDto.PeriodEndorsement},
			uzm_differenceendorsement={requestDto.DifferenceEndorsement}
		where uzm_loyaltycardId='{requestDto.Id}'
		select @lcCount= @@ROWCOUNT;

		Update uzm_customerendorsement set uzm_integrationstatus=1 where uzm_customerendorsementId='{requestDto.CustomerEndorsementId}';
		select @ceCount= @@ROWCOUNT;
		
		IF @lcCount = 0 OR @ceCount = 0
			BEGIN 
				ROLLBACK TRANSACTION 
				select 0 as result
		END
	END

COMMIT TRANSACTION 
select 1 as result
END TRY 

BEGIN CATCH 
	ROLLBACK TRANSACTION
	select 0 as result
END CATCH";
            return await dapperService.SaveQueryParam<object, LoyaltyCardUpdateResponseDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Müşteri Sadakat Kartı SQL komutu ile güncelleme
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardUpdateResponseDto>> LoyaltyCardDiscountFieldsUpdateWithSqlAsync(LoyaltyCardUpdateDto requestDto)
        {
            var query = $@"
BEGIN TRANSACTION
BEGIN TRY
	BEGIN
		declare @lcCount int; set @lcCount=0;
		Update [uzm_loyaltycard] set
			uzm_validdiscountratevakko={(requestDto.ValidDiscountRateVakko.HasValue ? requestDto.ValidDiscountRateVakko : "null")},
			uzm_validdiscountratevr={(requestDto.ValidDiscountRateVr.HasValue ? requestDto.ValidDiscountRateVr : "null")},
			uzm_validdiscountratewcol={(requestDto.ValidDiscountRateWcol.HasValue ? requestDto.ValidDiscountRateWcol : "null")},

			uzm_amountforuppersegmentvakko={(requestDto.AmountForUpperSegmentVakko.HasValue ? requestDto.AmountForUpperSegmentVakko : "null")},
			uzm_amountforuppersegmentvr={(requestDto.AmountForUpperSegmentVr.HasValue ? requestDto.AmountForUpperSegmentVr : "null")},
			uzm_amountforuppersegmentwcol={(requestDto.AmountForUpperSegmentWcol.HasValue ? requestDto.AmountForUpperSegmentWcol : "null")},

            uzm_uppersegmentdiscountpercentvakko ={(requestDto.UpperSegmentDiscountPercentVakko.HasValue ? requestDto.UpperSegmentDiscountPercentVakko : "null")},
			uzm_uppersegmentdiscountpercentvr={(requestDto.UpperSegmentDiscountPercentVr.HasValue ? requestDto.UpperSegmentDiscountPercentVr : "null")},
			uzm_uppersegmentdiscountpercentwcol={(requestDto.UpperSegmentDiscountPercentWcol.HasValue ? requestDto.UpperSegmentDiscountPercentWcol : "null")}
		where uzm_loyaltycardId='{requestDto.Id}'
		select @lcCount= @@ROWCOUNT;
		
		IF @lcCount = 0
			BEGIN 
				ROLLBACK TRANSACTION 
				select 0 as result
		END
	END

COMMIT TRANSACTION 
select 1 as result
END TRY 

BEGIN CATCH 
	ROLLBACK TRANSACTION
	select 0 as result
END CATCH";
            return await dapperService.SaveQueryParam<object, LoyaltyCardUpdateResponseDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Kart numarası kullanarak sadakat kart id sini bulur. 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardDto>> GetLoyaltyCardByCardNoAsync(string cardNo)
        {
            var query = $@"SELECT top 1
                        	uzm_loyaltycardId,
                        	uzm_cardnumber,
                        	uzm_cardtypedefinitionid,
                        	uzm_cardtypedefinitionidName,
                        	uzm_contactidName,
                        	uzm_erpId,
                        	uzm_statuscode,
                        	uzm_validdiscountratevakko,
                            uzm_validdiscountratevr,
                            uzm_validdiscountratewcol,
                        	uzm_periodendorsement,
                        	uzm_validendorsement,
                        	uzm_turnoverendorsement,
                        	uzm_amountforuppersegmentvakko,
                        	uzm_amountforuppersegmentvr,
                        	uzm_amountforuppersegmentwcol,
                        	uzm_uppersegmentdiscountpercentvakko,
                        	uzm_uppersegmentdiscountpercentvr,
                        	uzm_uppersegmentdiscountpercentwcol,
                        	CASE WHEN statecode=0 THEN 'A' ELSE 'P' END AS State,
                        	DATEADD(HOUR, 3, ModifiedOn) AS modifiedon,
							uzm_carddiscountid
                        FROM [uzm_loyaltycard] WITH(NOLOCK)
                        WHERE uzm_cardnumber='{cardNo}' and statecode=0 AND uzm_statuscode=1";

            return await dapperService.GetItemParam<object, LoyaltyCardDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// contact id bilgisi ile sadakat kart sorgulama
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<Response<LoyaltyCardDto>> GetBusinessUnitIdByStoreCodeAsync(LoyaltyCardRequestDto requestDto)
        {
            var query = String.Format(@"
             select top 1 b.BusinessUnitId as uzm_storeid from  KahveDunyasi_MSCRM..BusinessUnitBase b with(nolock) where  b.uzm_accountcode=@StoreCode ");
            var resService = await dapperService.GetItemParam<object, LoyaltyCardDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        /// <summary>
        /// Personeller için crm içerisinde tanımlı indirim oranını çekme
        /// P tipli kayıtların sadakat kartı olmadığı için model manipule edilerek gönderilecektir
        /// </summary>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardDto>> PersonelDiscountGetAsync(string contactName, string erpId)
        {
            var model = new Response<LoyaltyCardDto>();

            var getLoyaltyCardResponse = await GetPersonelDiscountAsync(contactName, erpId);
            if (getLoyaltyCardResponse.Success)
            {
                model.Success = true;
                model.Message = CommonStaticConsts.Message.Success;
                model.Data = getLoyaltyCardResponse.Data;
            }
            else
            {
                model.Data = null;
                model.Success = false;
                model.Error = getLoyaltyCardResponse.Error != null ? getLoyaltyCardResponse.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.LoyaltyCardGetError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.LoyaltyCardStaticConsts.LC0001 });
                model.Message = getLoyaltyCardResponse.Message;

                return model;
            }

            return model;
        }

        private async Task<Response<LoyaltyCardDto>> GetPersonelDiscountAsync(string contactName, string erpId)
        {
            var responseModel = new Response<LoyaltyCardDto>();
            var wagescaleListRes = await wageScaleService.ValidWageScaleGetList();
            if (wagescaleListRes.Success)
            {
                var cardTypeIdPersonel = new Guid("FADE4F41-872B-EE11-9169-00505685232B"); // Personel Kart Tipi
                var minWageScaleVakko = wageScaleService.GetBiggerThanZeroDiscountMinWagescaleByCardType(wagescaleListRes.Data, cardTypeIdPersonel);
                if (minWageScaleVakko.Success)
                {
                    responseModel.Success = true;
                    responseModel.Message = CommonStaticConsts.Message.Success;
                    responseModel.Data = new LoyaltyCardDto
                    {
                        uzm_validdiscountratevakko = minWageScaleVakko.Data.uzm_discountrate,
                        uzm_uppersegmentdiscountpercentvakko = 0,
                        uzm_amountforuppersegmentvakko = 0,

                        uzm_validdiscountratevr = minWageScaleVakko.Data.uzm_discountrate,
                        uzm_uppersegmentdiscountpercentvr = 0,
                        uzm_amountforuppersegmentvr = 0,

                        uzm_validdiscountratewcol = minWageScaleVakko.Data.uzm_discountrate,
                        uzm_uppersegmentdiscountpercentwcol = 0,
                        uzm_amountforuppersegmentwcol = 0,

                        uzm_cardtypedefinitionid = new Guid("FADE4F41-872B-EE11-9169-00505685232B"),
                        uzm_cardtypedefinitionidname = "P",
                        uzm_validendorsement = 0,
                        uzm_turnoverendorsement = 0,
                        uzm_periodendorsement = 0,
                        uzm_endorsementtype = 0,
                        uzm_contactidname = contactName,
                        uzm_erpid = erpId,
                        State = "A"
                    };
                }
                else
                {
                    responseModel.Data = null;
                    responseModel.Success = false;
                    responseModel.Error = minWageScaleVakko.Error != null ? minWageScaleVakko.Error :
                        (new ErrorModel { Description = CommonStaticConsts.Message.CustomerEndorsementDataProcessingWagescaleFindError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.GeneralErrorStaticConsts.V002 });
                    responseModel.Message = minWageScaleVakko.Message;

                    return responseModel;
                }
            }
            else
            {
                responseModel.Data = null;
                responseModel.Success = false;
                responseModel.Error = wagescaleListRes.Error != null ? wagescaleListRes.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.CustomerEndorsementDataProcessingWagescaleFindError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.GeneralErrorStaticConsts.V002 });
                responseModel.Message = wagescaleListRes.Message;

                return responseModel;
            }

            return responseModel;
        }
    }
}
