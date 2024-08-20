using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.DataSource;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Erp;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Loyalty;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Rest;
using UzmanCrm.CrmService.Domain.Entity.CRM.Contact;
using UzmanCrm.CrmService.Domain.Entity.OVM;

namespace UzmanCrm.CrmService.Application.Service.ContactService
{
    public class ContactService : IContactService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly ICRMService crmService;
        private readonly IRestService restService;
        private readonly IPhoneService phoneService;
        private readonly IUserService userService;
        private readonly IBusinessUnitService businessService;
        private readonly IEmailService emailService;



        private readonly string Erp_Api_Login_Token_Url;
        private readonly string Erp_Api_Login_User;
        private readonly string Erp_Customer_Save_Url;
        private readonly string Loyalty_Api_Url;

        public ContactService(IMapper mapper,
            ICRMService crmService,
            IDapperService dapperService,
            ILogService logService, IRestService restService, IPhoneService phoneService,
            IUserService userService, IBusinessUnitService businessService, IEmailService emailService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
            this.crmService = crmService;
            this.restService = restService;
            this.phoneService = phoneService;
            this.userService = userService;
            this.businessService = businessService;
            this.emailService = emailService;


            Erp_Customer_Save_Url = ContactHelper.Get_VakkoServiceLink("Erp_Customer_Save_Url");
            Erp_Api_Login_Token_Url = ContactHelper.Get_VakkoServiceLink("Erp_Api_Login_Token_Url");
            Erp_Api_Login_User = ContactHelper.Get_VakkoServiceLink("Erp_Api_Login_User");
            Loyalty_Api_Url = ContactHelper.Get_VakkoServiceLink("Loyalty_Api_Url");
        }

        public async Task<Response<ContactByErpIdDto>> GetContactItemByErpIdAsync(string erpId)
        {
            var query = String.Format($@"
            select top(1) ContactId, MobilePhone, EMailAddress1, uzm_ErpId, uzm_customertype, FullName
            from ContactBase with(nolock) where uzm_ErpId='{erpId}' and statecode=0
            order by ModifiedOn desc");
            var resService = await dapperService.GetItemParam<object, ContactByErpIdDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }
        public async Task<Response<ContactByErpAndCardIdDto>> GetContactByErpAndCardId(ContactByErpAndCardIdDto requestDto)
        {
            var query = String.Format(@"
            select c.ContactId,c.uzm_ErpId,uzm_loyaltycardid
            from ContactBase c with(nolock) 
            where c.uzm_ErpId=@uzm_erpid and uzm_loyaltycardid=@uzm_loyaltycardid
            and c.statecode=0");
            var resService = await dapperService.GetItemParam<object, ContactByErpAndCardIdDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        /// <summary>
        /// Müşteri kaydında sadakat kart bilgisini güncelleme metodu
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<UpdateResponse>> UpdateLoyaltyCardInfoOnContactAsync(LoyaltyCardInfoOnContactRequest requestDto)
        {
            var responseModel = new Response<UpdateResponse>();
            try
            {
                var crmContactUpdateStatement = "";
                var ovmCustomerUpdateStatement = "";

                switch (requestDto.uzm_loyaltycardtype)
                {
                    case CardTypeEnum.W:
                        // Kart tipi W ise vkk_loyaltycardidwcol ile ilişkili alanlar kullanılır
                        crmContactUpdateStatement = $"vkk_loyaltycardidwcol = @uzm_loyaltycardid, vkk_loyaltycardnowcol = @uzm_loyaltycardno, vkk_loyaltycardtypewcol = '{requestDto.uzm_loyaltycardtype.ToString()}'";
                        ovmCustomerUpdateStatement = $"CardNoWcol = @uzm_loyaltycardno, CardTypeWcol = '{requestDto.uzm_loyaltycardtype.ToString()}'";
                        break;
                    default:
                        // Kart tipi V veya R ise uzm_loyaltycardid ile ilişkili alanlar kullanılır
                        crmContactUpdateStatement = $"uzm_loyaltycardid = @uzm_loyaltycardid, uzm_loyaltycardno = @uzm_loyaltycardno, uzm_loyaltycardtype ='{requestDto.uzm_loyaltycardtype.ToString()}'";
                        ovmCustomerUpdateStatement = $"CardNo = @uzm_loyaltycardno, CardType ='{requestDto.uzm_loyaltycardtype.ToString()}'";
                        break;
                }

                var query = String.Format($@"
                Update OVM..Customer set {ovmCustomerUpdateStatement} where CrmId = @contactid

                Update ContactBase set {crmContactUpdateStatement} where ContactId = @contactid
                select @@ROWCOUNT as OutputResult");
                responseModel = await dapperService.SaveQueryParam<object, UpdateResponse>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(UpdateLoyaltyCardInfoOnContactAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<UpdateResponse>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.ContactInsertUpdateError + ex.ToString(), "")));
            }

            return responseModel;
        }
        public async Task<Response<List<ContactDto>>> GetContactListForCardExceptionDiscount(CardExceptionContactRequestDto requestDto)
        {
            var whereStatement = "";
            if (requestDto.CardNo.IsNotNullAndEmpty())
                whereStatement = $"lc.uzm_cardnumber = '{requestDto.CardNo}'";
            else if (requestDto.CardNo.IsNullOrEmpty() && requestDto.ErpId.IsNotNullAndEmpty())
                whereStatement = $"cb.uzm_ErpId = '{requestDto.ErpId}'";
            else if (requestDto.CardNo.IsNotNullAndEmpty() && requestDto.ErpId.IsNotNullAndEmpty())
                whereStatement = $"lc.uzm_cardnumber = '{requestDto.CardNo}' OR cb.uzm_ErpId='{requestDto.ErpId}'";
            else if (requestDto.EmailAddress1.IsNotNullAndEmpty() && requestDto.MobilePhone.IsNotNullAndEmpty())
                whereStatement = $"cb.EmailAddress1 = '{requestDto.EmailAddress1}'";
            else if (requestDto.EmailAddress1.IsNotNullAndEmpty())
                whereStatement = $"cb.EmailAddress1 = '{requestDto.EmailAddress1}'";
            else if (requestDto.MobilePhone.IsNotNullAndEmpty())
                whereStatement = $"cb.MobilePhone = '{requestDto.MobilePhone}'";
            else if (requestDto.MobilePhone.IsNotNullAndEmpty() && requestDto.EmailAddress1.IsNotNullAndEmpty())
                whereStatement = $"cb.MobilePhone = '{requestDto.MobilePhone}' OR cb.uzm_ErpId='{requestDto.EmailAddress1}'";
            var query = ($@"SELECT cb.[ContactId]
                                  ,cb.[FirstName]
                                  ,cb.[LastName]
                                  ,cb.[FullName]
                                  ,cb.[BirthDate]
                                  ,cb.[Description]
                                  ,cb.[EmployeeId]
                                  ,cb.[GenderCode]
                                  ,cb.[EMailAddress1]
                                  ,cb.[CreatedOn]
                                  ,cb.[MobilePhone]
                                  ,cb.[Telephone1]
                                  ,cb.[StateCode]
                                  ,cb.[StatusCode]o
                                  ,cb.[uzm_ErpId]
                                  ,cb.[uzm_customerno]
                                  ,cb.[uzm_customertype]
                                  ,cb.[uzm_cardtype]
                                  ,cb.[uzm_loyaltycardid]
                                  ,lc.[uzm_cardnumber]
                                    FROM ContactBase cb WITH(NOLOCK)
                                      LEFT JOIN uzm_loyaltycardBase lc ON cb.ContactId = lc.uzm_contactid
                                      WHERE {whereStatement}");
            var resService = await dapperService.GetListByParamAsync<CardExceptionContactRequestDto, ContactDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
            return resService;
        }
        /// <summary>
        /// Crm de şirketler ile ana müşteri kaydı n:n -> alt müşteri tablosu kullanılarak ilişkilendirilmiş.
        /// Elimizde olan şirketin contact id bilgisi(contactidTwo) ile ara tabloya istek atıp,
        ///     normal contact tablosunu ana kaydın contact id bilgisi(contactidOne) ile join'liyoruz ve istediğimiz bilgileri çekiyoruz.
        /// </summary>
        /// <param name="subContactId"></param>
        /// <returns></returns>
        public async Task<Response<ContactByErpIdDto>> GetMainContactItemBySubContactIdAsync(string subContactId)
        {
            var query = String.Format($@"
            SELECT TOP (1) cb.ContactId, cb.MobilePhone, cb.EMailAddress1, cb.uzm_ErpId, cb.uzm_customertype
            FROM uzm_contact_contact cc  WITH(NOLOCK)
            JOIN [KahveDunyasi_MSCRM].[dbo].[ContactBase] cb  WITH(NOLOCK) ON cc.contactidOne = cb.ContactId
            WHERE cc.contactidTwo='{subContactId}'
            order by cb.ModifiedOn desc");
            var resService = await dapperService.GetItemParam<object, ContactByErpIdDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        #region SearchCustomerAsync...

        /// <summary>
        /// Crm ile senkron çalışan OVM Customer tablosundaki search işlemini yapan metot 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<List<CustomerDto>>> SearchCustomerAsync(GetCustomerRequestDto requestDto)
        {
            var response = new Response<List<CustomerDto>>();
            try
            {

                #region ParametersFormat...

                string whereCondition = "";
                requestDto.MobilePhone = requestDto.MobilePhone ?? "";

                #endregion

                if (!String.IsNullOrEmpty(requestDto.MobilePhone))
                {
                    whereCondition += (string.IsNullOrEmpty(whereCondition) ?
                                       string.Format("mobilephone = N'{0}' ", requestDto.MobilePhone) : string.Format(" and mobilephone = N'{0}' ", requestDto.MobilePhone));
                }

                var query = StaticConsts.SearchSqlFieldText + whereCondition + StaticConsts.SearchSqlWhereEnd;

                var resService = await dapperService.GetListByParamAsync<object, CustomerDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD)).ConfigureAwait(false);


                if (!resService.Success)
                {
                    resService = await dapperService.GetListByParamAsync<object, CustomerDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD)).ConfigureAwait(false);
                }


                response = mapper.Map<Response<List<CustomerDto>>>(resService);
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                  this.GetType().Name,
                  nameof(SearchCustomerAsync),
                  CompanyEnum.KD,
                  LogTypeEnum.Response,
                  ex
                  );
                return await Task.FromResult(ResponseHelper.SetSingleError<List<CustomerDto>>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.SearchCustomerError + ex.ToString(), "")));
            }

            return response;
        }

        #endregion

        /// <summary>
        /// Erp Id bilgisi ile müşteriyi bulma metodu.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<GetCustomerResponseDto>> GetCustomerSearchAsync(GetCustomerRequestDto requestDto)
        {
            var responseModel = new Response<GetCustomerResponseDto>();
            try
            {
                string whereCondition = "";

                if (!String.IsNullOrEmpty(requestDto.MobilePhone))
                {
                    whereCondition += (string.IsNullOrEmpty(whereCondition) ?
                                       string.Format("mobilephone = N'{0}' ", requestDto.MobilePhone) : string.Format(" and mobilephone = N'{0}' ", requestDto.MobilePhone));
                }

                var query = StaticConsts.SearchSqlFieldText + whereCondition + StaticConsts.SearchSqlWhereEnd;

                var responseGetDp = await dapperService.GetItemParam<GetCustomerRequestDto, ContactDto>(query, requestDto,
                    GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

                responseModel = mapper.Map<Response<ContactDto>, Response<GetCustomerResponseDto>>(responseGetDp);


            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetCustomerSearchEcomIdAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<GetCustomerResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.SqlGetError + ex.ToString(), "")));
            }

            return responseModel;
        }

        #region GetCustomerByEcomId...

        /// <summary>
        /// Eticaret Id bilgisi ile müşteriyi bulma metodu.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<Response<List<SearchEcomIdResponse>>> GetCustomerSearchEcomIdAsync(SearchEcomIdRequest request)
        {
            var responseModel = new Response<List<SearchEcomIdResponse>>();
            try
            {

                var query = String.Format($@"select d.uzm_customerdatasourceId as Id , d.uzm_datasourceid as DatasourceId,
                d.uzm_customerid as CrmId, d.uzm_customerexternalid as CustomerExternalId,
                d.uzm_phone as Phone, uzm_email as Email
                from  KahveDunyasi_MSCRM..uzm_customerdatasourceBase d with(nolock)
                inner join KahveDunyasi_MSCRM..BusinessUnitBase b with(nolock) on d.uzm_datasourceid= b.BusinessUnitId
                where d.uzm_customerexternalid= @EcomId and ISNULL(d.uzm_unusedflag,0)=0
                and b.uzm_accountcode=@ChannelId and d.uzm_customerid is not null
                ");
                responseModel = await dapperService.GetListByParamAsync<SearchEcomIdRequest, SearchEcomIdResponse>(query, request, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);


            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetCustomerSearchEcomIdAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<List<SearchEcomIdResponse>>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.SqlGetError + ex.ToString(), "")));
            }

            return responseModel;
        }



        /// <summary>
        /// Crm Id bilgisi ile müşteriyi bulma metodu.
        /// </summary>
        /// <param name="crmId"></param>
        /// <returns></returns>
        private async Task<Response<GetCustomerByEcomIdResponseDto>> GetCustomerByCrmIdAsync(Guid? crmId)
        {
            var responseModel = new Response<GetCustomerByEcomIdResponseDto>();
            try
            {

                var query = String.Format($@"select c.ContactId as CrmId,c.uzm_ErpId as ErpId,c.FirstName as FirstName,c.LastName as LastName,c.BirthDate as  BirthDate,
                c.uzm_loyaltycardno as CardNo, c.uzm_loyaltycardtype as CardType, c.vkk_loyaltycardnowcol as CardNoWcol, c.vkk_loyaltycardtypewcol as CardTypeWCol,
                CASE
                    WHEN c.StateCode = 0 THEN 'Aktif'
                    WHEN c.StateCode = 1 THEN 'Pasif'
                    ELSE 'Pasif'
                END AS StatusEnum,
                c.ModifiedOn as LastUpdatedDate,c.uzm_customertype as CustomerType,c.GenderCode as GenderId,
                c.uzm_iskvkk as IsKvkk
                from ContactBase c with(nolock)
                where c.ContactId='{crmId}'
                order by c.ModifiedOn desc ");
                responseModel = await dapperService.GetItemParam<object, GetCustomerByEcomIdResponseDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);


            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetCustomerByCrmIdAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<GetCustomerByEcomIdResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.SqlGetError + ex.ToString(), "")));
            }

            return responseModel;
        }

        /// <summary>
        /// Crm Id bilgisi ile müşteri telefon listesini çekme metodu.
        /// </summary>
        /// <param name="crmId"></param>
        /// <returns></returns>
        private async Task<Response<List<ContactPhoneModelDto>>> GetCustomerPhoneListByCrmIdAsync(Guid? crmId)
        {
            var responseModel = new Response<List<ContactPhoneModelDto>>();
            try
            {

                var query = String.Format($@"	-- 1: izinsiz , 0: İzinli
				select uzm_customerphonenumber as PhoneNumber,				
				 CASE
                    WHEN p.uzm_phonepermission = 0 THEN 1
                    WHEN p.uzm_phonepermission = 1 THEN 0
                    ELSE 0
                END AS SmsPermit,
				 CASE
                    WHEN p.uzm_callpermission = 0 THEN 1
                    WHEN p.uzm_callpermission = 1 THEN 0
                    ELSE 0
                END AS CallPermit,
				dateadd(HOUR, 3, p.CreatedOn) as CreatedDate ,dateadd(HOUR, 3, p.ModifiedOn) as UpdatedDate
                from uzm_customerphone p with(nolock)
                where p.uzm_customerId='{crmId}'
                order by p.ModifiedOn desc 
");
                responseModel = await dapperService.GetListByParamAsync<object, ContactPhoneModelDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);


            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetCustomerByCrmIdAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<List<ContactPhoneModelDto>>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.SqlGetError + ex.ToString(), "")));
            }

            return responseModel;
        }

        /// <summary>
        /// Crm Id bilgisi ile müşteri email listesini çekme metodu.
        /// </summary>
        /// <param name="crmId"></param>
        /// <returns></returns>
        private async Task<Response<List<ContactEmailModelDto>>> GetCustomerEmailListByCrmIdAsync(Guid? crmId)
        {
            var responseModel = new Response<List<ContactEmailModelDto>>();
            try
            {

                var query = String.Format($@"	-- 1: izinsiz , 0: İzinli
				select uzm_emailaddress as EmailAddress,				
				 CASE
                    WHEN p.uzm_emailpermission = 0 THEN 1
                    WHEN p.uzm_emailpermission = 1 THEN 0
                    ELSE 0
                END AS EmailPermit,			 
				dateadd(HOUR, 3, p.CreatedOn) as CreatedDate ,dateadd(HOUR, 3, p.ModifiedOn) as UpdatedDate
                from uzm_customeremailBase p with(nolock)
                where p.uzm_customerId='{crmId}'
                order by p.ModifiedOn desc 
");
                responseModel = await dapperService.GetListByParamAsync<object, ContactEmailModelDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);


            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetCustomerByCrmIdAsync),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<List<ContactEmailModelDto>>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.SqlGetError + ex.ToString(), "")));
            }

            return responseModel;
        }

        /// <summary>
        /// EcomId ve ChannelId bilgise göre müşteri getirme işlemini yapan metot 
        /// </summary>
        /// <param name="EcomId"></param>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        public async Task<Response<GetCustomerByEcomIdResponseDto>> GetCustomerByEcomIdAsync(string EcomId, EcomChannelTypeEnum ChannelId)
        {
            var request = new SearchEcomIdRequest() { ChannelId = ChannelId, EcomId = EcomId };

            var response = new Response<GetCustomerByEcomIdResponseDto>();
            var result_SearchEcomData = await GetCustomerSearchEcomIdAsync(request);
            if (result_SearchEcomData.Success)
            {
                var crmId = result_SearchEcomData.Data[0].CrmId;

                var result_ContactInfo = await GetCustomerByCrmIdAsync(crmId);

                var result_ContactPhoneList = await GetCustomerPhoneListByCrmIdAsync(crmId);
                var result_ContactEmailList = await GetCustomerEmailListByCrmIdAsync(crmId);

                #region ContactInfo_Phone_Email Mapping...

                if (result_ContactPhoneList.Data.Count > 0)
                {
                    result_ContactInfo.Data.PhoneNumber = result_ContactPhoneList.Data[0].PhoneNumber;
                    result_ContactInfo.Data.SmsPermit = result_ContactPhoneList.Data[0].SmsPermit;
                    result_ContactInfo.Data.CallPermit = result_ContactPhoneList.Data[0].CallPermit;
                }

                if (result_ContactEmailList.Data.Count > 0)
                {
                    result_ContactInfo.Data.EmailAddress = result_ContactEmailList.Data[0].EmailAddress;
                    result_ContactInfo.Data.EmailPermit = result_ContactEmailList.Data[0].EmailPermit;
                }

                #endregion

                return result_ContactInfo;
            }
            else
            {
                response.Data = null;
                response.Success = false;
                response.Error = result_SearchEcomData.Error != null ? result_SearchEcomData.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.ContactGetEcomIdError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.SearchErrorStaticConsts.S012 });
                response.Message = result_SearchEcomData.Message;

            }


            return response;
        }

        #endregion

        #region SaveCustomerEcomAsync

        /// <summary>
        /// EcomId ve ChannelId bilgise göre müşteri getirme işlemini yapan metot 
        /// </summary>
        /// <param name="EcomId"></param>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        public async Task<Response<SaveCustomerResponseDto>> SaveCustomerAsync(SaveCustomerRequestDto requestDto)
        {

            var response = new Response<SaveCustomerResponseDto>();

            var request = new GetCustomerRequestDto() { MobilePhone = requestDto.MobilePhone };
            var result_SearchData = await GetCustomerSearchAsync(request);
            if (result_SearchData.Success)
                requestDto.CrmId = result_SearchData.Data.CrmId;

            //Person
            if (!string.IsNullOrEmpty(requestDto.PersonNo))
            {
                var portalUserResponse = await userService.GetEmployeeNumberAsync(requestDto.PersonNo, CompanyEnum.KD);
                if (portalUserResponse.Success)
                    requestDto.PersonId = portalUserResponse.Data.uzm_employeeid;
            }

            //Store
            if (!string.IsNullOrEmpty(requestDto.StoreCode))
            {
                var storeResponse = await businessService.GetStoreCodeAsync(requestDto.StoreCode, requestDto.Company);
                if (storeResponse.Success)
                    requestDto.StoreId = storeResponse.Data.uzm_storeid;
            }

            #region MAPPING

            var contactMappingModel = mapper.Map<SaveCustomerRequestDto, Contact>(requestDto);

            #endregion MAPPING

            #region INSERT_UPDATE_PROCESS


            if (requestDto.CrmId is null || requestDto.CrmId == Guid.Empty)
            {

                //Yeni müşteri oluşuyor
                var contactInsertResponse = await InsertContactAsync(contactMappingModel, CompanyEnum.KD);
                if (contactInsertResponse.Success && !string.IsNullOrEmpty(contactMappingModel.emailaddress1))
                {
                    var emailSaveResponse = await emailService.EmailSaveAsync(new EmailSaveRequestDto
                    {
                        CustomerCrmId = contactInsertResponse?.Data.ToString(),
                        EmailPermission = requestDto.EmailPermit,
                        ChannelId = requestDto.ChannelId,
                        Organization = OrganizationEnum.TR,
                        PersonNo = requestDto.PersonNo,
                        PersonId = requestDto.PersonId,
                        EmailAddress = requestDto.EmailAddress,
                        StatusEnum = StatusType.Aktif,
                        StoreCode = requestDto.StoreCode,
                        StoreId = requestDto.StoreId,
                        Company = CompanyEnum.KD,
                        EmailOptinDate = requestDto.EmailPermitDate

                    });

                }
                else
                {

                    response.Data = null;
                    response.Success = false;
                    response.Error = new ErrorModel { Description = CommonStaticConsts.Message.ContactInsertUpdateError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.CustomerStaticConsts.C001 };
                    response.Message = contactInsertResponse.Message;
                    return response;

                }

                response.Success = true;
                response.Message = "Success";
                response.Data.CrmId = contactInsertResponse?.Data;
                response.Data.Type = CreateType.Create;

            }
            else
            {


                var contactInsertResponse = await InsertContactAsync(contactMappingModel, CompanyEnum.KD);
                if (contactInsertResponse.Success)
                {
                    if (!string.IsNullOrEmpty(requestDto.EmailAddress))
                    {
                        var emailSaveResponse = await emailService.EmailSaveAsync(new EmailSaveRequestDto
                        {
                            CustomerCrmId = contactInsertResponse?.Data.ToString(),
                            EmailPermission = requestDto.EmailPermit,
                            ChannelId = requestDto.ChannelId,
                            Organization = OrganizationEnum.TR,
                            PersonNo = requestDto.PersonNo,
                            PersonId = requestDto.PersonId,
                            EmailAddress = requestDto.EmailAddress,
                            StatusEnum = StatusType.Aktif,
                            StoreCode = requestDto.StoreCode,
                            StoreId = requestDto.StoreId,
                            Company = CompanyEnum.KD,
                            EmailOptinDate = requestDto.EmailPermitDate

                        }); ;

                        //var dataSourceSaveResponse = await DataSourceSaveAsync(mappingModel);


                    }
                    response.Success = true;
                    response.Message = "Success";
                    response.Data.CrmId = contactInsertResponse.Data;
                    response.Data.Type = CreateType.Update;

                }
                else
                {
                    response.Error = contactInsertResponse.Error;
                }

            }



            #endregion INSERT_UPDATE_PROCESS

            #region RESPONSE_CHECK
            if (response.Success &&
               (string.IsNullOrEmpty(response.Data.CrmId.ToString())))
            {
                response.Data = null;
                response.Success = false;
                response.Error = new ErrorModel
                {
                    Description = CommonStaticConsts.Message.ContactResponseError,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    ErrorCode = ErrorStaticConsts.CustomerStaticConsts.C002
                };
                response.Message = CommonStaticConsts.Message.Unsuccess;
                return response;
            }
            #endregion INSERT_UPDATE_PROCESS


            return response;
        }



        private async Task<Response<CreateCustomerFormResponse>> CreateCustomerFormAsync(CrmCustomerFormRequestDto request)
        {
            var responseModel = new Response<CreateCustomerFormResponse>();
            //
            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                    this.GetType().Name,
                    nameof(CreateCustomerFormAsync),
                    request.Company,
                    LogTypeEnum.Request,
                    request
                    );

            try
            {
                var mappingModel = mapper.Map<CrmCustomerFormRequestDto, CustomerPermission>(request);

                var resp = crmService.Save(mappingModel, "uzm_customerpermissions", "uzm_customerpermissions", request.Company);

                await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                        this.GetType().Name,
                        nameof(CreateCustomerFormAsync),
                        request.Company,
                        LogTypeEnum.Response,
                        resp
                        );

                responseModel.Data.Id = resp.Data;
                responseModel.Data.CustomerCrmId = request.CrmId;
                responseModel.Success = resp.Success;
                responseModel.Message = resp.Message;
                responseModel.Error = resp.Error;

            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(DataSourceSaveAsync),
                   request.Company,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<CreateCustomerFormResponse>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                    CommonStaticConsts.Message.CreateCustomerFormError + ex.ToString(), "")));

            }

            return responseModel;
        }


        #endregion

        #region DataSourceSaveAsync

        public async Task<Response<DataSourceSaveResponse>> DataSourceSaveAsync(ContactSaveRequestDto requestDto)
        {

            var responseModel = new Response<DataSourceSaveResponse>();

            var dataSourceDto = mapper.Map<DataSourceDto>(requestDto);

            var resService = await GetDataSourceItemAsync(requestDto);
            if (resService.Success)
            {
                responseModel.Success = resService.Success;
                responseModel.Message = resService.Message;
                responseModel.Data.CustomerCrmId = resService.Data.uzm_customerid;
                responseModel.Data.Id = resService.Data.uzm_customerdatasourceid;
                return responseModel;
            }

            try
            {
                var entityModel = mapper.Map<DataSource>(dataSourceDto);
                entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);

                var result = crmService.Save<DataSource>(entityModel, "uzm_customerdatasource", "uzm_customerdatasource", requestDto.Company);

                await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                   this.GetType().Name,
                   nameof(DataSourceSaveAsync),
                   requestDto.Company,
                   LogTypeEnum.Response,
                   result
                   );

                responseModel.Data.Id = result.Data;
                responseModel.Data.CustomerCrmId = dataSourceDto.uzm_customerid;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;

            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(DataSourceSaveAsync),
                   requestDto.Company,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<DataSourceSaveResponse>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                    CommonStaticConsts.Message.DataSourceSaveError + ex.ToString(), "")));

            }

            return responseModel;
        }

        private async Task<Response<DataSourceDto>> GetDataSourceItemAsync(ContactSaveRequestDto requestDto)
        {
            var query = String.Format(@"SELECT [uzm_customerdatasourceId],[uzm_customerid],[uzm_customerexternalid]
            FROM uzm_customerdatasourceBase with(nolock) where uzm_customerid=@CrmId and uzm_customerexternalid=@EcomId and statecode=0");
            var resService = await dapperService.GetItemParam<object, DataSourceDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(requestDto.Company)).ConfigureAwait(false);

            return resService;

        }


        #endregion


        /// <summary>
        /// Search contact duplicate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<Response<SearchDuplicateResponseModel>> SearchDuplicateAsync(ContactSaveRequestDto request)
        {
            var model = new Response<SearchDuplicateResponseModel>();

            //Kural 1: ErpId
            if (!string.IsNullOrEmpty(request.CrmId.ToString()))
            {
                var query = String.Format(@"SELECT 
                                                CrmId,
                                                CustomerType
                                            FROM
                                                Customer 
                                            WHERE 
                                                ErpId = @ErpId
                                                AND StateCode = 0");

                var resDup = await dapperService.GetItemParam<object, Customer>(query, new { ErpId = request.ErpId }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D011;
                    return model;
                }
            }

            //Kural 1: CrmId
            if (!string.IsNullOrEmpty(request.CrmId.ToString()))
            {
                var query = String.Format(@"SELECT 
                                                CrmId,
                                                CustomerType
                                            FROM
                                                Customer 
                                            WHERE 
                                                CrmId = @CrmId
                                                AND StateCode = 0");


                var resDup = await dapperService.GetItemParam<object, Customer>(query, new { CrmId = request.CrmId }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D012;
                    return model;
                }
            }

            // OK - Kural 2: Ad - Soyad - Cep Telefonu - Doğum tarihi
            if (!string.IsNullOrEmpty(request.FirstName) &&

                !string.IsNullOrEmpty(request.LastName) &&
                !string.IsNullOrEmpty(request.BirthDate?.ToString()))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType 
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Gsm1 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND BirthDate = @BirthDate 
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm2 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND BirthDate = @BirthDate 
                                              AND Statu=0 AND CustomerType!='Z' ");

                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    PhoneNumber = "",
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D002;
                    return model;
                }
            }

            // OK - Kural 3: Ad - Soyad - Cep Telefonu - Email
            if (!string.IsNullOrEmpty(request.FirstName) &&

                !string.IsNullOrEmpty(request.LastName) &&
                !string.IsNullOrEmpty(request.Email?.EmailAddress))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType  
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Gsm1 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND Email1 = @Email
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm1 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND Email2 = @Email 
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm2 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND Email1 = @Email
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm2 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND Email2 = @Email 
                                              AND Statu=0 AND CustomerType!='Z' ");


                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    PhoneNumber = "",
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email.EmailAddress
                }, GeneralHelper.GetOvmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D003;
                    return model;
                }

            }

            // OK - Kural 4: Ad - Soyad - Doğum Tarihi - Email
            if (!string.IsNullOrEmpty(request.FirstName) &&
                !string.IsNullOrEmpty(request.BirthDate?.ToString()) &&
                !string.IsNullOrEmpty(request.LastName) &&
                !string.IsNullOrEmpty(request.Email?.EmailAddress))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType  
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Email1 = @Email 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND BirthDate = @BirthDate 
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Email2 = @Email 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName 
                                              AND BirthDate = @BirthDate 
                                              AND Statu=0 AND CustomerType!='Z' ");
                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    Email = request.Email.EmailAddress,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D004;
                    return model;
                }
            }

            // OK - Kural 5: Ad - Soyad - Cep Telefonu
            if (!string.IsNullOrEmpty(request.FirstName) &&
                !string.IsNullOrEmpty(request.LastName))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType  
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Gsm1 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm2 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName
                                              AND Statu=0 AND CustomerType!='Z' ");
                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    PhoneNumber = "",
                    FirstName = request.FirstName,
                    LastName = request.LastName
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D005;
                    return model;
                }
            }

            // OK - Kural 6: Ad - Soyad - Email
            if (!string.IsNullOrEmpty(request.FirstName) &&
                !string.IsNullOrEmpty(request.LastName) &&
                !string.IsNullOrEmpty(request.Email?.EmailAddress))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType  
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Email1 = @Email 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName
                                              AND Statu=0 AND CustomerType!='Z' 
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Email2 = @Email 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName
                                              AND Statu=0 AND CustomerType!='Z' ");
                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email.EmailAddress
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D007;
                    return model;
                }
            }

            // OK - Kural 7: Ad - Cep Telefonu - Email
            if (!string.IsNullOrEmpty(request.FirstName) &&

                !string.IsNullOrEmpty(request.Email?.EmailAddress))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType  
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Gsm1 = @PhoneNumber 
                                              AND FirstName = @FirstName
                                              AND Email1 = @Email
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm1 = @PhoneNumber 
                                              AND FirstName = @FirstName
                                              AND Email2 = @Email 
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm2 = @PhoneNumber 
                                              AND FirstName = @FirstName
                                              AND Email1 = @Email
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm2 = @PhoneNumber 
                                              AND FirstName = @FirstName
                                              AND Email2 = @Email 
                                              AND Statu=0 AND CustomerType!='Z' ");

                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    PhoneNumber = "",
                    FirstName = request.FirstName,
                    Email = request.Email.EmailAddress
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D008;
                    return model;
                }
            }

            // OK - Kural 8: Ad - Cep Telefonu - Doğum Tarihi
            if (!string.IsNullOrEmpty(request.FirstName) &&

                !string.IsNullOrEmpty(request.BirthDate?.ToString()))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType  
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Gsm1 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND BirthDate = @BirthDate
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Gsm2 = @PhoneNumber 
                                              AND FirstName = @FirstName 
                                              AND BirthDate = @BirthDate
                                              AND Statu=0 AND CustomerType!='Z' ");
                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    PhoneNumber = "",
                    FirstName = request.FirstName,
                    BirthDate = request.BirthDate
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D009;
                    return model;
                }
            }

            // OK - Kural 9: Ad - Doğum Tarihi - Email
            if (!string.IsNullOrEmpty(request.FirstName) &&
                !string.IsNullOrEmpty(request.BirthDate?.ToString()) &&
                !string.IsNullOrEmpty(request.Email?.EmailAddress))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType 
                                            FROM 
                                              Customer 
                                            WHERE 
                                              Email1 = @Email 
                                              AND FirstName = @FirstName 
                                              AND BirthDate = @BirthDate
                                              AND Statu=0 AND CustomerType!='Z'
                                            UNION
                                            SELECT
                                                CrmId,
                                                CustomerType 
                                            FROM
                                                Customer 
                                            WHERE 
                                              Email2 = @Email 
                                              AND FirstName = @FirstName 
                                              AND BirthDate = @BirthDate
                                              AND Statu=0 AND CustomerType!='Z' ");
                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    FirstName = request.FirstName,
                    Email = request.Email.EmailAddress,
                    BirthDate = request.BirthDate
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D010;
                    return model;
                }
            }

            // OK - Kural 10: Ad - Soyad - Doğum Tarihi
            if (!string.IsNullOrEmpty(request.FirstName) &&
                !string.IsNullOrEmpty(request.LastName) &&
                !string.IsNullOrEmpty(request.BirthDate?.ToString()))
            {
                var query = String.Format(@"SELECT 
                                              CrmId,
                                              CustomerType  
                                            FROM 
                                              Customer 
                                            WHERE 
                                              BirthDate = @BirthDate 
                                              AND FirstName = @FirstName 
                                              AND LastName = @LastName
                                              AND Statu=0 AND CustomerType!='Z'");
                var resDup = await dapperService.GetItemParam<object, Customer>(query, new
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate
                }, GeneralHelper.GetOvmConnectionStringByCompany(request.Company)).ConfigureAwait(false);

                if (resDup.Success && resDup.Data != null && resDup.Data.crmid.HasValue)
                {
                    model.Success = true;
                    model.Data.CrmId = resDup.Data.crmid.Value;
                    model.Data.CustomerType = resDup.Data.customertype;
                    model.Data.DuplicateCode = ErrorStaticConsts.DuplicateErrorStaticConsts.D006;
                    return model;
                }
            }

            return model;

        }
        private async Task<Response<ErpSaveCustomerResponseModel>> ErpSaveCustomer(ContactSaveRequestDto request)
        {
            var responseModel = new Response<ErpSaveCustomerResponseModel>();
            var model = new ErpSaveCustomerRequestModel
            {
                birthDateString = request.BirthDate?.ToString("yyyy-MM-dd"),
                creatorPerson = null,
                creatorChannel = request.Location,
                name = request.FirstName,
                surname = request.LastName,
                genderId = Convert.ToInt32(request.GenderId - 1).ToString(),
                gsm = "90" + "",
                isKvkk = request.IsKvkk == true ? 1 : 0,
                crm_id = null,
                tckn = "",
                erp_id = null,
                taxNumber = null,
                homeCountryId = 0, // CountryId
                homeCityId = 0,// CityId
                homeDistrictId = 0, // NeigborhoodId
                homeCountyId = 0, // DistrictId
                homeAdress1 = null, // AdressLine
                cardType = request.CardType,
                customerType = request.CustomerType

            };


            try
            {
                var tokenResponse = await ErpApiLoginToken();
                if (tokenResponse.Success)
                {
                    var response = await restService.SendRequest<ErpSaveCustomerResponseModel>(model, Erp_Customer_Save_Url, tokenResponse.Data.access_token);
                    if (!response.Success)
                    {

                        await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                                           this.GetType().Name,
                                           nameof(ErpSaveCustomer),
                                           CompanyEnum.KD,
                                           LogTypeEnum.Response,
                                            "response.Success=false " + response.Message
                                           );
                    }
                    else
                    {

                        if (response.Data.error)
                            return await Task.FromResult(ResponseHelper.SetSingleError<ErpSaveCustomerResponseModel>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, "erpSaveResponse.Success=false " + response.Data.message, "")));


                        return response;
                    }

                }
                else
                {

                    return await Task.FromResult(ResponseHelper.SetSingleError<ErpSaveCustomerResponseModel>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                        "tokenResponse.Success=false " + tokenResponse.Message, "")));
                }
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(ErpApiLoginToken),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<ErpSaveCustomerResponseModel>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                    CommonStaticConsts.Message.ErpSaveCustomerError + ex.ToString(), "")));
            }


            /*

             {"birthDateString":"1973-07-07","creatorPerson":null,"crm_id":"00000000-0000-0000-0000-000000000000",
            "email":null,"erp_id":null,"genderId":"0","gsm":"05425263477","homeAdress1":null,"homeAdress2":null,
            "homeCityId":0,"homeCountryId":0,"homeCountyId":0,"homeDistrictId":0,"homePostCode":null,"homeTel":null,
            "isKvkk":1,"name":"AYSEN","surname":"TAHTAKİLTAHTAKILIC","taxAdministration":null,"taxNumber":null,"tckn":"",
            "creatorChannel":"2891","cardType":"R","customerType":"M"}
             */
            return responseModel;
        }
        private async Task<Response<ErpTokenResponseModel>> ErpApiLoginToken()
        {
            var responseModel = new Response<ErpTokenResponseModel>();
            try
            {
                var plainTextBytes = Encoding.UTF8.GetBytes(Erp_Api_Login_User);
                var encodedText = Convert.ToBase64String(plainTextBytes);
                var response = await restService.SendRequest<ErpTokenResponseModel>(null, Erp_Api_Login_Token_Url, encodedText,
                    Method.Post, "Basic");
                if (!response.Success)
                {
                    await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                                       this.GetType().Name,
                                       nameof(ErpApiLoginToken),
                                       CompanyEnum.KD,
                                       LogTypeEnum.Response,
                                        "response.Success=false " + response.Message
                                       );
                }
                else
                    return response;

            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(ErpApiLoginToken),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<ErpTokenResponseModel>(new ErrorModel(System.Net.HttpStatusCode.BadGateway,
                    CommonStaticConsts.Message.ErpApiLoginError + ex.ToString(), "")));
            }

            return responseModel;
        }

        private async Task<Response<Customer>> InsertOvmCustomerAsync(Customer customer, CompanyEnum company)
        {
            //await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
            //    this.GetType().Name,
            //    nameof(InsertOvmCustomerAsync),
            //    company,
            //    LogTypeEnum.Request,
            //    customer);

            var query = @"INSERT INTO
                          Customer 
                          (
                            CustomerId,
                            FirstName,
                            LastName,
                            GenderId,
                            Tckno,
                            Gsm1,
                            ContactableGsm1,
                            Gsm2,
                            ContactableGsm2,
                            Email1,
                            ContactableEmail1,
                            Email2,
                            ContactableEmail2,
                            CountryId,
                            CityId,
                            DisctrictId,
                            NeighborhoodId,
                            AdressLine,
                            BirthDate,
                            ChannelId,
                            IsOffline,
                            IsOnline,
                            CreatedLocation,
                            CreatedUser,
                            CreatedDate,
                            PersonelNumber,
                            CrmCreatedDate,
                            ErpId,CustomerNo,CustomerType,EcomId
                          )
                            OUTPUT	INSERTED.CustomerId,INSERTED.CustomerType
                          VALUES
                          (
                            NEWID(),
                            @FirstName,
                            @LastName,
                            @GenderId,
                            @Tckno,
                            @Gsm1,
                            @ContactableGsm1,
                            @Gsm2,
                            @ContactableGsm2,
                            @Email1,
                            @ContactableEmail1,
                            @Email2,
                            @ContactableEmail2,
                            @CountryId,
                            @CityId,
                            @DisctrictId,
                            @NeighborhoodId,
                            @AdressLine,
                            @BirthDate,
                            @ChannelId,
                            @IsOffline,
                            @IsOnline,
                            @CreatedLocation,
                            @CreatedUser,
                            GETDATE(),
                            @PersonelNumber,
                            GETDATE(),
                            @ErpId,@CustomerNo,@CustomerType,@EcomId
                          );";

            var response = await dapperService.SaveQueryParam<Customer, Customer>(query, customer, GeneralHelper.GetOvmConnectionStringByCompany(company));

            //await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
            //    this.GetType().Name,
            //    nameof(InsertOvmCustomerAsync),
            //    company,
            //    LogTypeEnum.Response,
            //    response
            //    );

            return response;
        }
        private async Task<Response<Guid>> InsertContactAsync(Contact contact, CompanyEnum company)
        {

            //await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
            //    this.GetType().Name,
            //    nameof(InsertContactAsync),
            //    company,
            //    LogTypeEnum.Request,
            //    contact
            //    );

            var res = crmService.Save(contact, "contact", "contact", company);

            //await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
            //    this.GetType().Name,
            //    nameof(InsertContactAsync),
            //    company,
            //    LogTypeEnum.Response,
            //    res
            //    );

            return res;
        }
        private async Task<Response<int>> UpdateOvmCustomerByCustomerIdAsync(Customer customer, CompanyEnum company)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(UpdateOvmCustomerByCustomerIdAsync),
                company,
                LogTypeEnum.Request,
                customer
                );

            var query = $@"UPDATE 
                          Customer 
                        SET 
                          CrmId = @CrmId, 
                          FirstName = @FirstName, 
                          LastName = @LastName, 
                          GenderId = @GenderId, 
                          Tckno = @Tckno, 
                          Gsm1 = @Gsm1, 
                          ContactableGsm1 = @ContactableGsm1, 
                          Gsm2 = @Gsm2, 
                          ContactableGsm2 = @ContactableGsm2, 
                          Email1 = @Email1, 
                          ContactableEmail1 = @ContactableEmail1, 
                          Email2 = @Email2, 
                          ContactableEmail2 = @ContactableEmail2, 
                          CountryId = @CountryId, 
                          CityId = @CityId, 
                          DisctrictId = @DisctrictId, 
                          NeighborhoodId = @NeighborhoodId, 
                          AdressLine = @AdressLine, 
                          BirthDate = @BirthDate, 
                          ChannelId = @ChannelId, 
                          IsOffline = @IsOffline, 
                          IsOnline = @IsOnline, 
                          CreatedLocation = @CreatedLocation,
                          PersonelNumber = @PersonelNumber,
                          UpdatedDate = GETDATE(),
                          [Statu] = {(customer.statu.HasValue ? customer.statu.Value ? 1 : 0 : 0)}
                        WHERE 
                          CustomerId = @CustomerId
                        SELECT @@ROWCOUNT";

            var response = await dapperService.SaveQueryParam<Customer, int>(query, customer, GeneralHelper.GetOvmConnectionStringByCompany(company));

            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(UpdateOvmCustomerByCustomerIdAsync),
                company,
                LogTypeEnum.Response,
                response
                );

            return response;
        }
        private async Task<Response<CustomerDto>> GetCustomerByCrmIdAsync(Guid? crmId, CompanyEnum company)
        {
            if (crmId == Guid.Empty)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<CustomerDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest, CommonStaticConsts.Message.CustomerIdIsNotValidFormat, ErrorStaticConsts.FormatErrorStaticConsts.F001)));
            }

            var query = String.Format(@"SELECT  CustomerId,CrmId,CardNo,FirstName,LastName,Gsm1,Gsm2,ErpId,CustomerType
			                            FROM
                                            Customer WITH(NOLOCK)
			                            WHERE
                                            CrmId = @CrmId
                                            AND Statu = 0");

            var resService = await dapperService.GetItemParam<object, Customer>(query, new { CrmId = crmId }, GeneralHelper.GetOvmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<CustomerDto>>(resService);

            return response;
        }

        private async Task<Response<CustomerDto>> GetCustomerByErpIdAsync(string erpId, CompanyEnum company)
        {
            var query = String.Format(@"SELECT  CustomerId,CrmId,CardNo,FirstName,LastName,Gsm1,Gsm2,ErpId
			                            FROM
                                            Customer WITH(NOLOCK)
			                            WHERE
                                            ErpId = @ErpId
                                            AND Statu = 0");

            var resService = await dapperService.GetItemParam<object, Customer>(query, new { ErpId = erpId }, GeneralHelper.GetOvmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<CustomerDto>>(resService);

            return response;
        }

        private async Task<Response<SaveLoyaltyCardResponseModel>> LoyaltyApi_SaveLoyaltyCard(SaveLoyaltyCardRequestModel request)
        {
            var responseModel = new Response<SaveLoyaltyCardResponseModel>();

            try
            {
                var tokenResponse = await LoyaltyApi_LoginToken();
                if (tokenResponse.Success)
                {
                    var response = await restService.SendRequest<SaveLoyaltyCardResponseModel>(request, Loyalty_Api_Url + "/loyalty/save-loyalty-card", tokenResponse.Data.Data.Token);
                    if (!response.Success && !response.Message.Contains(":\"LC0002\""))
                    {
                        await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                                           this.GetType().Name,
                                           nameof(LoyaltyApi_SaveLoyaltyCard),
                                           CompanyEnum.KD,
                                           LogTypeEnum.Response,
                                            "response.Success=false " + response.Message
                                           );
                    }
                    else
                        return response;
                }
                else
                {

                    return await Task.FromResult(ResponseHelper.SetSingleError<SaveLoyaltyCardResponseModel>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                        "tokenResponse.Success=false " + tokenResponse.Message, "")));
                }
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(LoyaltyApi_SaveLoyaltyCard),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<SaveLoyaltyCardResponseModel>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                    CommonStaticConsts.Message.ErpSaveCustomerError + ex.ToString(), "")));
            }


            /*             
             {"birthDateString":"1973-07-07","creatorPerson":null,"crm_id":"00000000-0000-0000-0000-000000000000",
            "email":null,"erp_id":null,"genderId":"0","gsm":"05425263477","homeAdress1":null,"homeAdress2":null,
            "homeCityId":0,"homeCountryId":0,"homeCountyId":0,"homeDistrictId":0,"homePostCode":null,"homeTel":null,
            "isKvkk":1,"name":"AYSEN","surname":"TAHTAKİLTAHTAKILIC","taxAdministration":null,"taxNumber":null,"tckn":"",
            "creatorChannel":"2891","cardType":"R","customerType":"M"}
             */


            return responseModel;
        }
        private async Task<Response<AuthenticateResponseModel>> LoyaltyApi_LoginToken()
        {
            var responseModel = new Response<AuthenticateResponseModel>();
            try
            {
                var response = await restService.SendRequest<AuthenticateResponseModel>(new AuthenticateRequestModel(), Loyalty_Api_Url + "/login/authenticate");
                if (!response.Success)
                {
                    await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                                       this.GetType().Name,
                                       nameof(LoyaltyApi_LoginToken),
                                       CompanyEnum.KD,
                                       LogTypeEnum.Response,
                                        "response.Success=false " + response.Message
                                       );
                }
                else
                    return response;

            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(LoyaltyApi_LoginToken),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                    );

                return await Task.FromResult(ResponseHelper.SetSingleError<AuthenticateResponseModel>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                    CommonStaticConsts.Message.LoyaltyApiLoginError + ex.ToString(), "")));
            }

            return responseModel;
        }

        //select d.uzm_customerdatasourceId as Id , d.uzm_datasourceid as DatasourceId,
        //d.uzm_customerid as CrmId , d.uzm_customerexternalid as CustomerExternalId,
        //d.uzm_phone as Phone, uzm_email as Email
        //from  KahveDunyasi_MSCRM..uzm_customerdatasourceBase d with(nolock)
        //inner join KahveDunyasi_MSCRM..BusinessUnitBase b with(nolock) on d.uzm_datasourceid= b.BusinessUnitId
        //where d.uzm_customerexternalid= '324265'
        //and ISNULL(d.uzm_unusedflag,0)=0
        //and b.uzm_accountcode='2890'
        //and d.uzm_customerid= '{824BEDCE-8BA9-44D8-97AC-B993191DB496}'
    }
}

