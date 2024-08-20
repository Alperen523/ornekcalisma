using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.CardExceptionDiscount;

namespace UzmanCrm.CrmService.Application.Service.CardExceptionDiscountService
{
    public class CardExceptionDiscountService : ICardExceptionDiscountService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly ICRMService crmService;
        private readonly ILoyaltyCardService loyaltyCardService;
        public CardExceptionDiscountService(IMapper mapper,
            ICRMService crmService,
            IDapperService dapperService,
            ILogService logService,
            ILoyaltyCardService loyaltyCardService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
            this.crmService = crmService;
            this.loyaltyCardService = loyaltyCardService;
        }

        /// <summary>
        /// CardExceptionDiscount segment save for CRM
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<CardExceptionDiscountSaveResponseDto>> CardExceptionDiscountSaveAsync(CardExceptionDiscountRequestDto requestDto)
        {
            var responseModel = new Response<CardExceptionDiscountSaveResponseDto>();

            var modelDto = mapper.Map<CardExceptionDiscountDto>(requestDto);
            try
            {
                var loyaltyCardRes = await loyaltyCardService.GetLoyaltyCardByIdAsync((Guid)requestDto.LoyaltyCardId);
                if (loyaltyCardRes.Success)
                {
                    var resService = await GetCardExceptionDiscountItemByErpIdAsync(loyaltyCardRes.Data.uzm_erpid);
                    if (resService.Success)
                        modelDto.uzm_carddiscountId = resService.Data.uzm_carddiscountId;

                    var entityModel = mapper.Map<CardExceptionDiscount>(modelDto);
                    entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);
                    entityModel.vkk_erpid = loyaltyCardRes.Data.uzm_erpid;

                    var result = crmService.Save<CardExceptionDiscount>(entityModel, "uzm_carddiscount", "uzm_carddiscount", Common.Enums.CompanyEnum.KD);

                    responseModel.Data.Id = result.Data;
                    responseModel.Success = result.Success;
                    responseModel.Message = result.Message;
                    responseModel.Error = result.Error;
                }
                else
                {
                    responseModel.Success = loyaltyCardRes.Success;
                    responseModel.Message = loyaltyCardRes.Message;
                    responseModel.Error = loyaltyCardRes.Error;
                }
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(CardExceptionDiscountSaveAsync),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<CardExceptionDiscountSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.CardExceptionDiscountSaveError + ex.ToString(), "")));
            }

            return responseModel;
        }

        /// <summary>
        /// Kart Statü bilgisi "İptal Edildi" olmayıp eşleşen aktif kayıt var mı sorgusudur.
        /// İptal Edildi durumu harici tüm durumlarda istisna kaydı güncellenecektir.
        /// İptal Edildi durumu varsa yeni İstisna Tanımı kaydı oluşur
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        private async Task<Response<CardExceptionDiscountDto>> GetCardExceptionDiscountItemByErpIdAsync(string erpId)
        {
            var query = String.Format($@"SELECT uzm_carddiscountId FROM uzm_carddiscount WITH(NOLOCK)
                WHERE vkk_erpid='{erpId}' AND statecode=0 AND uzm_statuscode!=4");

            var resService = await dapperService.GetItemParam<object, CardExceptionDiscountDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        /// <summary>
        /// İstek atılan gün itibariyle geçerli olan Onaylandı ve Kullanımda olan Aktif istisna tanımı var mı kontrolü yapar. ErpId bilgisi ile filtreler
        /// </summary>
        /// <param name="erpId"></param>
        /// <returns></returns>
        public async Task<Response<CardExceptionDiscountDto>> GetCardExceptionDiscountDetailByErpIdAsync(string erpId)
        {
            var query = String.Format($@"
                SELECT uzm_carddiscountId, uzm_discountrate
                FROM uzm_carddiscount WITH(NOLOCK) 
                WHERE vkk_erpid='{erpId}' AND (GETDATE() BETWEEN uzm_startdate AND uzm_enddate)
                AND statecode=0 AND uzm_approvalstatus=3 AND uzm_statuscode=1");
            var resService = await dapperService.GetItemParam<object, CardExceptionDiscountDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        public async Task<Response<CardExceptionDiscountAndContactDto>> GetCardDiscount(Guid? cardExceptionDiscountId)
        {
            var queryCardDiscount = $@"SELECT TOP 1 cd.uzm_carddiscountid
                                         ,cd.statecode
                                         ,cd.statecodename  
                                         ,cd.statuscode  
                                         ,cd.statuscodename  
                                         ,cd.uzm_approvalexplanation  
                                         ,cd.uzm_approvalstatus  
                                         ,cd.uzm_approvalstatusname  
                                         ,cd.uzm_approvedby  
                                         ,cd.uzm_approvedbyname  
                                         ,cd.uzm_arrivalchannel  
                                         ,cd.uzm_arrivalchannelname  
                                         ,cd.uzm_cardclasssegmentid  
                                         ,cd.uzm_cardclasssegmentidname  
                                         ,cd.uzm_customergroupid  
                                         ,cd.uzm_customergroupidname  
                                         ,cd.uzm_demanddate  
                                         ,cd.uzm_demandeduser  
                                         ,cd.uzm_demandedusername
                                         ,cd.uzm_demandstore  
                                         ,cd.uzm_demandstorename
                                         ,cd.uzm_description  
                                         ,cd.uzm_discountrate  
                                         ,cd.uzm_enddate  
                                         ,cd.uzm_loyaltycardid  
                                         ,cd.uzm_loyaltycardidname  
                                         ,cd.uzm_name
                                         ,cd.uzm_startdate  
                                         ,cd.uzm_statuscode  
                                         ,cd.uzm_statuscodename
	                                   	 ,lc.uzm_cardnumber
	                                   	 ,lc.uzm_erpid
                                         ,lc.uzm_mobilephone
                                         ,lc.uzm_contactidname
                                         ,lc.uzm_validdiscountratevakko
                                         ,lc.uzm_validdiscountratewcol
                                         ,lc.uzm_validdiscountratevr
                                         ,lc.uzm_turnoverendorsement
                                         ,lc.uzm_periodendorsement
                                         ,pu.uzm_portaluserid
                                         ,pu.uzm_storeid
                                         ,pu.uzm_storeidname
                                         ,pu.uzm_organizationid
                                         ,pu.uzm_organizationidName
                                    FROM [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_carddiscount] cd
	                                LEFT JOIN [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_loyaltycard] lc ON lc.uzm_carddiscountid = cd.uzm_carddiscountid
	                                LEFT JOIN [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_portaluser] pu ON cd.uzm_demandeduser = pu.uzm_portaluserid
                                    WHERE cd.uzm_carddiscountid = '{cardExceptionDiscountId}'
                                    ORDER BY uzm_startdate desc";
            var responseModel = await dapperService.GetItemParam<CardExceptionDiscountRequestDto, CardExceptionDiscountAndContactDto>(queryCardDiscount, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
            
            return responseModel;
        }

        public async Task<Response<CardExceptionDiscountSaveResponseDto>> UpdateCardExceptionDiscountApprovalStatusAndExplanationAsync(CardApprovalStatusAndExplanationRequestDto requestDto)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Request", nameof(UpdateCardExceptionDiscountApprovalStatusAndExplanationAsync), CompanyEnum.KD, LogTypeEnum.Request, requestDto);
            var responseModel = new Response<CardExceptionDiscountSaveResponseDto>();
            try
            {
                var cardException = mapper.Map<CardExceptionDiscount>(requestDto);
                cardException = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, cardException);

                var result = crmService.Save<CardExceptionDiscount>(cardException, "uzm_carddiscount", "uzm_carddiscount", CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(UpdateCardExceptionDiscountApprovalStatusAndExplanationAsync),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<CardExceptionDiscountSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.CardExceptionDiscountSaveError + ex.ToString(), "")));
            }
            await logService.LogSave(Common.Enums.LogEventEnum.DbInfo, "Response", nameof(UpdateCardExceptionDiscountApprovalStatusAndExplanationAsync), CompanyEnum.KD, LogTypeEnum.Response, responseModel);
            return responseModel;
        }

        public async Task<Response<CardExceptionDiscountSaveResponseDto>> UpdateCardExceptionDiscountEndDateAndDiscountRateAsync(CardEndDateRequestDto requestDto)
        {
            var responseModel = new Response<CardExceptionDiscountSaveResponseDto>();
            try
            {
                var cardException = mapper.Map<CardExceptionDiscount>(requestDto);

                var result = crmService.Save<CardExceptionDiscount>(cardException, "uzm_carddiscount", "uzm_carddiscount", CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(UpdateCardExceptionDiscountEndDateAndDiscountRateAsync),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<CardExceptionDiscountSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.CardExceptionDiscountSaveError + ex.ToString(), "")));
            }

            return responseModel;
        }

        public async Task<Response<List<CardExceptionDiscountAndContactDto>>> GetCardExceptionDiscountsCreatedFromStore()
        {
            var queryCardDiscount = @$"SELECT cd.uzm_carddiscountId,
                                              cd.uzm_cardclasssegmentid,
                                              cd.uzm_discountrate,
                                              cd.uzm_demandeduser,
                                              cd.uzm_demanddate,
                                              cd.uzm_approvalstatus,
                                              cd.uzm_statuscode,
                                              cd.uzm_arrivalchannel,
                                              cd.uzm_customergroupid,
                                              cd.uzm_startdate,
                                              cd.uzm_enddate,
                                              cd.uzm_description,
                                              cd.uzm_approvedby,
                                              cd.uzm_approvalexplanation,
                                              pu.uzm_firstname,
                                              pu.uzm_lastname,
                                              puap.uzm_fullname,
                                              cg.uzm_name CustomerGroupName,
                                              ccs.uzm_name SegmentName
                                        FROM uzm_carddiscountBase cd WITH(NOLOCK)
                                        LEFT JOIN uzm_portaluserBase pu ON cd.uzm_demandeduser = pu.uzm_portaluserId
                                        LEFT JOIN uzm_portaluserBase puap ON cd.uzm_approvedby = puap.uzm_portaluserId
                                        LEFT JOIN uzm_customergroup cg ON cd.uzm_customergroupid = cg.uzm_customergroupId
                                        LEFT JOIN uzm_cardclasssegment ccs ON cd.uzm_cardclasssegmentid = ccs.uzm_cardclasssegmentId
                                          WHERE cd.CreatedOn >= DATEADD(day,-30, getdate()) 
                                            AND cd.CreatedOn <= getdate() AND uzm_arrivalchannel = '1'";
            var response = await dapperService.GetListAsync<CardExceptionDiscountAndContactDto>(queryCardDiscount, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Bitiş zamanına Kart Sınıfı varlığındaki bildirim süresi kadar zaman kalan istisna indirimi kayıtlarını listeler
        /// </summary>
        /// <param name="cardClassSegmentId"></param>
        /// <param name="remainingDayCount"></param>
        /// <returns></returns>
        public async Task<Response<List<GetCardExceptionDiscountsWillBeExpiredSoon_ResponseDto>>> GetCardExceptionDiscountsWillBeExpiredSoon(Guid cardClassSegmentId, int remainingPeriodCount)
        {
            var query = @$"
SELECT
	lc.uzm_contactidname,
	lc.uzm_erpid,
	cd.uzm_loyaltycardidname,
	cd.uzm_demandedusername,
	cd.uzm_demanddate,
	cd.uzm_cardclasssegmentidname,
	cd.uzm_customergroupidname,
	cd.uzm_arrivalchannelname,
	cd.uzm_discountrate,
	cd.uzm_startdate,
	cd.uzm_enddate,
	cd.uzm_description,
	cd.uzm_approvedbyname,
	cd.uzm_approvalexplanation,
	cd.uzm_demandstorename
FROM [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_carddiscount] cd WITH(NOLOCK)
LEFT JOIN [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_loyaltycard] lc WITH(NOLOCK) on lc.uzm_loyaltycardid=cd.uzm_loyaltycardid
WHERE cd.statecode=0 AND cd.uzm_approvalstatus=3 AND cd.uzm_statuscode=1 
AND uzm_cardclasssegmentid='{cardClassSegmentId}'
AND DATEDIFF(MONTH, GETDATE(), DATEADD(HOUR, 3, uzm_enddate)) = {remainingPeriodCount}";

            return await dapperService.GetListByParamAsync<object, GetCardExceptionDiscountsWillBeExpiredSoon_ResponseDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Bitiş tarihi(uzm_enddate) bugün, durumu(statecode) aktif ve statü bilgisi(uzm_statuscode) süresi doldu olmayan kart istisna indirimi kayıtlarını listeler
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<CardExceptionDiscountSaveResponseDto>>> GetExpiredTodayCardExceptionDiscounts()
        {
            var query = @$"SELECT cd.uzm_carddiscountid as Id
                           FROM [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_carddiscount] cd WITH(NOLOCK)
                           WHERE statecode=0 AND uzm_statuscode!=3 AND uzm_enddate<GETDATE() "; // önceki kod= AND DATEDIFF(DAY, GETDATE(), uzm_enddate) = 0 - bugünkü tarihten küçük ise geriye dönük bakması için eklendi
            return await dapperService.GetListByParamAsync<object, CardExceptionDiscountSaveResponseDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        public async Task<Response<CardExceptionDiscountDto>> CheckIfCardExceptionDiscountExistByErpId(string erpId)
        {
            var queryCardDiscount = $@"SELECT uzm_cardnumber
                                             ,uzm_erpid
                                             ,uzm_mobilephone
                                             ,uzm_contactidname
                                             ,uzm_carddiscountid
                                       FROM [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_loyaltycard]
                                       WHERE uzm_erpid = '{erpId}'";
            var response = await dapperService.GetItemParam<string, CardExceptionDiscountDto>(queryCardDiscount, erpId, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
            return response;
        }

        public async Task<Response<CardExceptionDiscountDto>> GetCardExceptionDiscountStatusAndApprovalStatusById(Guid? cardExceptionDiscountId)
        {
            var queryCardDiscount = $@"SELECT uzm_carddiscountid
                                       		 ,uzm_approvalstatus
                                       		 ,uzm_approvalstatusname
                                       		 ,uzm_statuscode
                                       		 ,uzm_statuscodename
                                       FROM [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_carddiscount]
                                       WHERE uzm_carddiscountid = '{cardExceptionDiscountId}'";
            var response = await dapperService.GetItemParam<Guid?, CardExceptionDiscountDto>(queryCardDiscount, cardExceptionDiscountId, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
            return response;
        }

        /// <summary>
        /// Toplu Onay Listesi bazlı, durumu(statecode) aktif ve istisna onay durumu,istisna durumu taslak olan kart istisna indirimi kayıtlarını listeler
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<CardExceptionDiscountDto>>> GetCardExceptionDiscountsByBatchApprovalListId(Guid? batchApprovalListId)
        {
            var query = @$"SELECT *
                           FROM [KahveDunyasi_MSCRM].[dbo].[Filtereduzm_carddiscount] cd WITH(NOLOCK)
                           WHERE statecode=0 AND uzm_approvalstatus=1 AND uzm_statuscode=0 AND vkk_batchapprovallistid = '{batchApprovalListId}'";
            return await dapperService.GetListByParamAsync<object, CardExceptionDiscountDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }
    }
}
