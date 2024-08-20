using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;

namespace UzmanCrm.CrmService.Application.Service.EndorsementService
{
    public class EndorsementService : IEndorsementService
    {
        private readonly IDapperService dapperService;
        private readonly IWageScaleService wageScaleService;
        private readonly IContactService contactService;
        private readonly ILoyaltyCardService loyaltyCardService;
        private readonly ILogService logService;

        public EndorsementService(
            IDapperService dapperService,
            IWageScaleService wageScaleService,
            IContactService contactService,
            ILoyaltyCardService loyaltyCardService,
            ILogService logService)
        {
            this.dapperService = dapperService;
            this.wageScaleService = wageScaleService;
            this.contactService = contactService;
            this.loyaltyCardService = loyaltyCardService;
            this.logService = logService;
        }

        /// <summary>
        /// Endorsement save for CRM
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<EndorsementSaveResponseDto>> EndorsementSaveAsync(EndorsementRequestDto requestDto)
        {
            var model = new Response<EndorsementSaveResponseDto>();

            var createEndorsementResponse = await CreateEndorsementAsync(requestDto);
            if (createEndorsementResponse.Success)
            {
                model.Success = true;
                model.Message = CommonStaticConsts.Message.Success;
                model.Data.Id = createEndorsementResponse.Data;
            }
            else
            {
                model.Data = null;
                model.Success = false;
                model.Error = createEndorsementResponse.Error != null ? createEndorsementResponse.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.EndorsementCreateError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.EndorsementStaticConsts.E0001 });
                model.Message = createEndorsementResponse.Message;
                return model;
            }

            return model;
        }


        private async Task<Response<Guid>> CreateEndorsementAsync(EndorsementRequestDto requestDto)
        {
            var queryInvoiceNo = @$"select [uzm_customerendorsementId],uzm_totalamount,uzm_giftcardamount from [uzm_customerendorsementBase] with (nolock) where [uzm_invoicenumber]='{requestDto.InvoiceNumber}' and statecode=0";
            var responseInvoiceNo = await dapperService.GetItemParam<object, CustomerEndorsementDto>(queryInvoiceNo, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
            if (responseInvoiceNo.Success && requestDto.BillType != BillTypeEnum.IsDelete && requestDto.BillType != BillTypeEnum.IsDeleteReturn)
            {
                var resp = new Response<Guid>()
                {
                    Error = new ErrorModel
                    {
                        Description = CommonStaticConsts.Message.EndorsementInvoiceExistError,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorCode = ErrorStaticConsts.EndorsementStaticConsts.E0002
                    }
                };
                return resp;
            }

            #region Sap den gelen silinecek satış faturası kontrol ediliyor. Iade olarak sistem kuyruğuna ekleme işlemi sağlanacak...

            //Sap den gelen silinecek fatura kontrol ediliyor. Tipine göre Iade veya Satış tipinde olarak sistem kuyruğuna ekleme işlemi sağlanacak.
            if (!responseInvoiceNo.Success && (requestDto.BillType == BillTypeEnum.IsDelete || requestDto.BillType == BillTypeEnum.IsDeleteReturn))
            {
                var resp = new Response<Guid>()
                {
                    Error = new ErrorModel
                    {
                        Description = CommonStaticConsts.Message.EndorsementIsDeleteError,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorCode = ErrorStaticConsts.EndorsementStaticConsts.E0004
                    }
                };
                return resp;
            }
            else if (responseInvoiceNo.Success && (requestDto.BillType == BillTypeEnum.IsDelete || requestDto.BillType == BillTypeEnum.IsDeleteReturn))
            {
                var oldInvoiceParam = requestDto.BillType == BillTypeEnum.IsDelete ? "_SATISIPTAL_" : "_IADEIPTAL_";
                var newInvoiceParam = requestDto.BillType == BillTypeEnum.IsDelete ? "_ISDELETESATIS_" : "_ISDELETEIADE_";
                var cancelBillType = requestDto.BillType == BillTypeEnum.IsDelete ? BillTypeEnum.ZR01 : BillTypeEnum.ZP01;

                responseInvoiceNo.Data.uzm_invoicenumber = $"{requestDto.InvoiceNumber}{oldInvoiceParam}{Guid.NewGuid()}";
                var invoiceUpdate = await UpdateEndorsementAsync(responseInvoiceNo.Data);
                if (invoiceUpdate.Success && invoiceUpdate.Data.OutputResult > 0)
                {
                    requestDto.InvoiceNumber = $"{requestDto.InvoiceNumber}{newInvoiceParam}{Guid.NewGuid()}";
                    requestDto.TotalAmount = (decimal)responseInvoiceNo.Data.uzm_totalamount;
                    requestDto.GiftCardAmount = (decimal)responseInvoiceNo.Data.uzm_giftcardamount;
                    requestDto.BillType = cancelBillType;

                    var IsDeleteEndorsementRes = await CreateEndorsementAsync(requestDto);
                    if (IsDeleteEndorsementRes.Success)
                    {
                        var endorsementWillBePassiveOldRes = await SetEndorsementWillBePassiveWithSqlAsync(1, (Guid)responseInvoiceNo.Data.uzm_customerendorsementid);
                        var endorsementWillBePassiveNewRes = await SetEndorsementWillBePassiveWithSqlAsync(1, IsDeleteEndorsementRes.Data);
                    }
                    return IsDeleteEndorsementRes;
                }
            }

            #endregion

            var contactid = Guid.Empty;
            var resContact = await contactService.GetContactItemByErpIdAsync(requestDto.ErpId);
            if (resContact.Success)
                contactid = (Guid)resContact.Data.ContactId;

            var loyaltycardid = Guid.Empty;
            var resLoyaltyCard = await loyaltyCardService.GetLoyaltyCardByCardNoAsync(requestDto.CardNo);
            if (resLoyaltyCard.Success)
                loyaltycardid = (Guid)resLoyaltyCard.Data.uzm_loyaltycardid;

            var queryCrm = @$"
INSERT INTO [uzm_customerendorsementBase]
           ([uzm_customerendorsementId]
		   ,[CreatedOn]
           ,[CreatedBy]
           ,[ModifiedOn]
           ,[ModifiedBy]
           ,[CreatedOnBehalfBy]
           ,[ModifiedOnBehalfBy]
           ,[OrganizationId]
           ,[statecode]
           ,[statuscode]
           ,[ImportSequenceNumber]
           ,[OverriddenCreatedOn]
           ,[TimeZoneRuleVersionNumber]
           ,[UTCConversionTimeZoneCode]
           ,[uzm_name]
           ,[uzm_invoicenumber]
           ,[uzm_ordernumber]
           ,[uzm_orderdate]
           ,[uzm_totalamount]
           ,[uzm_storecode]
           ,[uzm_transactionid]
           ,[uzm_erpid]
           ,[uzm_integrationstatus]
           ,[uzm_giftcardamount]
           ,[uzm_billtype]
           ,[uzm_cardno]
           ,[uzm_invoicedate]
           ,[uzm_contactid]
           ,[uzm_loyaltycardid])
	 OUTPUT INSERTED.uzm_customerendorsementId
     VALUES	 
          ( NEWID()
		   ,DATEADD(HOUR, -3, GETDATE())
           ,'A627B337-47CD-E811-8120-005056991930' -- crmadmin
           ,DATEADD(HOUR, -3, GETDATE())
           ,'A627B337-47CD-E811-8120-005056991930' -- crmadmin
           ,NULL
           ,NULL
           ,'30F6172F-47CD-E811-8120-005056991930'--VakkoCRM
           ,0
           ,1
           ,NULL
           ,NULL
           ,26
           ,134
           ,'{requestDto.InvoiceNumber}'
           ,'{requestDto.InvoiceNumber}'
           ,'{requestDto.OrderNumber}'
           ,'{requestDto.OrderDate}'
           ,{requestDto.TotalAmount}
           ,'{requestDto.StoreCode}'
           ,'{requestDto.TransactionId}'
           ,'{requestDto.ErpId}'
           ,0
           ,{requestDto.GiftCardAmount}
           ,{(int)requestDto.BillType}
           ,'{requestDto.CardNo}'
           ,'{requestDto.InvoiceDate}'
           ,{(contactid != Guid.Empty ? "\'" + contactid + "\'" : "NULL")}
           ,{(loyaltycardid != Guid.Empty ? "\'" + loyaltycardid + "\'" : "NULL")}
)";

            var responseCrm = await dapperService.SaveQueryParam<EndorsementRequestDto, Guid>(queryCrm, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));

            return responseCrm;
        }

        /// <summary>
        /// Get Customer Endorsement
        /// </summary>
        /// <param name="ErpId"></param>
        /// <returns></returns>
        public async Task<Response<LoyaltyCardDto>> EndorsementGetAsync(string erpId, CardTypeEnum cardType)
        {
            var model = new Response<LoyaltyCardDto>();

            var getCustomerEndorsementResponse = await GetCustomerEndorsement(erpId, cardType);
            if (getCustomerEndorsementResponse.Success)
            {
                model.Data = getCustomerEndorsementResponse.Data;
                model.Success = true;
                model.Message = CommonStaticConsts.Message.Success;
            }
            else
            {
                model.Data = null;
                model.Success = false;
                model.Error = getCustomerEndorsementResponse.Error != null ? getCustomerEndorsementResponse.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.EndorsementGetError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.EndorsementStaticConsts.E0003 });
                model.Message = getCustomerEndorsementResponse.Message;
                return model;
            }

            return model;
        }

        private async Task<Response<LoyaltyCardDto>> GetCustomerEndorsement(string erpId, CardTypeEnum cardType)
        {
            var cardTypeList = GeneralHelper.GetCardTypeFilter(cardType);

            var queryCustomerEndorsement = @$"SELECT
uzm_amountforuppersegmentvakko,
uzm_amountforuppersegmentvr,
uzm_amountforuppersegmentwcol,
uzm_cardtypedefinitionid,
uzm_cardtypedefinitionidName,
uzm_validdiscountratevakko,
uzm_validdiscountratevr,
uzm_validdiscountratewcol,
uzm_uppersegmentdiscountpercentvakko,
uzm_uppersegmentdiscountpercentvr,
uzm_uppersegmentdiscountpercentwcol,
CAST(uzm_validendorsement AS decimal(18,2)) uzm_validendorsement,
CAST(uzm_turnoverendorsement AS decimal(18,2)) uzm_turnoverendorsement,
CAST(uzm_periodendorsement AS decimal(18,2)) uzm_periodendorsement,
uzm_endorsementtype
FROM uzm_loyaltycard with(nolock)
WHERE uzm_erpId='{erpId}' AND statecode=0 AND uzm_statuscode=1 and uzm_cardtypedefinitionidName {cardTypeList}";

            return await dapperService.GetItemParam<object, LoyaltyCardDto>(queryCustomerEndorsement, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        public async Task<Response<List<CustomerEndorsementDto>>> GetUnintegratedEndorsementList()
        {
            var query = @$"
           SELECT [uzm_customerendorsementId]
          ,[CreatedOn]
          ,[CreatedBy]
          ,[ModifiedOn]
          ,[ModifiedBy]
          ,[CreatedOnBehalfBy]
          ,[ModifiedOnBehalfBy]
          ,[OrganizationId]
          ,[statecode]
          ,[statuscode]
          ,[VersionNumber]
          ,[ImportSequenceNumber]
          ,[OverriddenCreatedOn]
          ,[TimeZoneRuleVersionNumber]
          ,[UTCConversionTimeZoneCode]
          ,[uzm_name]
          ,[uzm_invoicenumber]
          ,[uzm_ordernumber]
          ,[uzm_orderdate]
          ,[uzm_totalamount]
          ,[uzm_storecode]
          ,[uzm_transactionid]
          ,[uzm_erpid]
          ,[uzm_integrationstatus]
          ,[uzm_giftcardamount]
          ,[uzm_billtype]
          ,[uzm_cardno]
      FROM [uzm_customerendorsementBase] with(nolock) where [uzm_integrationstatus]=0";

            return await dapperService.GetListByParamAsync<object, CustomerEndorsementDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Müşteri Ciro entegrasyon durumunu SQL komutu ile güncelleme
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<EndorsementSaveResponseDto>> SetEndorsementIntegrationStateWithSqlAsync(int integrationStatus, string integrationDescription, string customerEndorsementId, bool discountFixingFlag)
        {
            var query = $@"
Update uzm_customerendorsement 
set uzm_integrationstatus={integrationStatus}, uzm_integrationdescription='{integrationDescription}', uzm_discountfixingflag='{discountFixingFlag}'
where uzm_customerendorsementId='{customerEndorsementId}';
";
            return await dapperService.SaveQueryParam<object, EndorsementSaveResponseDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Müşteri kaydında sadakat kart bilgisini güncelleme metodu
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<UpdateResponse>> UpdateEndorsementAsync(CustomerEndorsementDto requestDto)
        {
            var responseModel = new Response<UpdateResponse>();
            try
            {
                var query = String.Format($@"
                  Update uzm_customerendorsementBase set uzm_invoicenumber = @uzm_invoicenumber
                  where uzm_customerendorsementId = @uzm_customerendorsementId
                  select @@ROWCOUNT as OutputResult");
                responseModel = await dapperService.SaveQueryParam<object, UpdateResponse>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(UpdateEndorsementAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<UpdateResponse>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.ContactInsertUpdateError + ex.ToString(), "")));
            }

            return responseModel;
        }

        /// <summary>
        /// Müşteri Ciro Pasifleşecek mi? durumunu SQL komutu ile güncelleme
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<UpdateResponse>> SetEndorsementWillBePassiveWithSqlAsync(int willBePassive, Guid customerEndorsementId)
        {
            var query = $@"
Update uzm_customerendorsement
set uzm_willbepassive={willBePassive}
where uzm_customerendorsementId='{customerEndorsementId}' select @@ROWCOUNT as OutputResult;
";
            return await dapperService.SaveQueryParam<object, UpdateResponse>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }
    }
}
