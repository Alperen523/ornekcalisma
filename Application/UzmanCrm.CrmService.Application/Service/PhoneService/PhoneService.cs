using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.DAL.Config.Application.Common;

namespace UzmanCrm.CrmService.Application.Service.PhoneService
{
    public class PhoneService : IPhoneService
    {
        private readonly IMapper _mapper;
        private readonly ICRMService _crmService;
        private readonly ILogService _logService;
        private readonly IDapperService _dapperService;
        private readonly IUserService _userService;
        private readonly IBusinessUnitService _businessService;

        public PhoneService(IMapper mapper, ICRMService crmService, ILogService logService, IDapperService dapperService,
            IUserService userService, IBusinessUnitService businessService)
        {
            _mapper = mapper;
            _crmService = crmService;
            _logService = logService;
            _dapperService = dapperService;
            _userService = userService;
            _businessService = businessService;
        }

        public async Task<Response<PhoneSaveResponse>> PhoneSaveAsync(PhoneSaveRequestDto requestDto)
        {
            var watch1 = new Stopwatch(); // GetPortalUserByPersonelNumberAsync ve  GetBusinessUnitByStoreCodeAsync  tamamlanma süresi
            var watch2 = new Stopwatch(); // GetPhoneItemAsync  tamamlanma süresi
            var watch3 = new Stopwatch(); // PhoneSave tamamlanma süresi
            var watch4 = new Stopwatch(); // UpdateContactPhoneAsync  tamamlanma süresi

            watch1.Start();
            var responseModel = new Response<PhoneSaveResponse>();

            var portalUserResponse = await _userService.GetEmployeeNumberAsync(requestDto.PersonNo, requestDto.Company);
            if (!portalUserResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<PhoneSaveResponse>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.PersonnelNotFound, ErrorStaticConsts.SearchErrorStaticConsts.S010)));
            }
            else
                requestDto.PersonId = portalUserResponse.Data.uzm_employeeid;

            var businessResponse = await _businessService.GetStoreCodeAsync(requestDto.StoreCode, requestDto.Company);
            if (!businessResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<PhoneSaveResponse>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.LocationNotFound, ErrorStaticConsts.SearchErrorStaticConsts.S011)));
            }
            else
                requestDto.StoreId = businessResponse.Data.uzm_storeid;


            var phoneDto = _mapper.Map<PhoneDto>(requestDto);

            //Eski genel izni Sms ile besledik.
            phoneDto.uzm_phonepermission = (bool)requestDto.SmsPermit ? false : true; // SmsPermit , 0 : İzinli , 1 : İzinsiz
            phoneDto.uzm_callpermission = (bool)requestDto.CallPermit ? false : true; // CallPermit , 0 : İzinli , 1 : İzinsiz
            phoneDto.uzm_whatsapppermission = phoneDto.uzm_phonepermission; // Eski servisteki özelik bu bölümede eklendi, Sms izni ile WhatsApp izni aynı olması kuralı.
            watch1.Stop();
            watch2.Start();
            var resService = await GetPhoneItemAsync(requestDto);
            if (resService.Success)
            {
                phoneDto.uzm_customerphoneid = resService.Data.uzm_customerphoneid;
                phoneDto.uzm_createdlocationid = null;
                phoneDto.uzm_createdpersonid = null;
            }
            watch2.Stop();
            try
            {
                watch3.Start();
                var entityModel = _mapper.Map<Domain.Entity.CRM.Contact.Phone>(phoneDto);
                entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);

                var result = _crmService.Save<Domain.Entity.CRM.Contact.Phone>(entityModel, "uzm_customerphone", "uzm_customerphone", requestDto.Company);
                watch3.Stop();
                await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(PhoneSaveAsync),
                 requestDto.Company,
                 LogTypeEnum.Response,
                 result
                 );

                responseModel.Data.Id = result.Data;
                responseModel.Data.CustomerCrmId = phoneDto.uzm_customerid;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;

                watch4.Start();
                if (result.Success)
                {
                    var updateContactPhone = await UpdateContactPhoneAsync(requestDto);
                }
                watch4.Start();

                //Log Stopwatch : 
                var LogStopwatch = (!string.IsNullOrEmpty(requestDto.PhoneNumber) ? requestDto.PhoneNumber : "No-PhoneNumber") +
                    @" watch1 : " + watch1.Elapsed.TotalSeconds + " - watch2 : " + watch2.Elapsed.TotalSeconds + " - watch3 : " + watch3.Elapsed.TotalSeconds + " - watch4 : " + watch4.Elapsed.TotalSeconds;

                await _logService.LogSave(Common.Enums.LogEventEnum.DbWarning,
                        this.GetType().Name,
                        "PhoneSaveAsync-Stopwatch",
                        CompanyEnum.KD,
                        LogTypeEnum.Response,
                        LogStopwatch
                        );
            }
            catch (Exception ex)
            {
                await _logService.LogSave(Common.Enums.LogEventEnum.DbError,
                                    this.GetType().Name,
                                    nameof(PhoneSaveAsync),
                                    requestDto.Company,
                                    LogTypeEnum.Response,
                                    ex
                                );

                return await Task.FromResult(ResponseHelper.SetSingleError<PhoneSaveResponse>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError,
                    CommonStaticConsts.Message.PhoneSaveError + ex.ToString(), ErrorStaticConsts.GeneralErrorStaticConsts.V003)));

            }

            return responseModel;
        }

        private async Task<Response<PhoneDto>> GetPhoneItemAsync(PhoneSaveRequestDto requestDto)
        {
            var query = String.Format(@"select uzm_customerphoneId from uzm_customerphoneBase with(nolock) where uzm_customerphonenumber=@PhoneNumber and uzm_customerId=@CustomerCrmId and statecode=0");
            var resService = await _dapperService.GetItemParam<object, PhoneDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(requestDto.Company)).ConfigureAwait(false);

            return resService;

        }

        public async Task<Response<PhoneDto>> GetPhoneByGsmAsync(string gsm, CompanyEnum company)
        {
            var query = String.Format(@"SELECT 
                                          uzm_customerphoneId,
	                                      uzm_customerId,
	                                      uzm_customerphonenumber
                                        FROM 
                                          uzm_customerphoneBase with(nolock) 
                                        WHERE 
                                          uzm_customerphonenumber = @Gsm 
                                          AND statecode = 0");
            var resService = await _dapperService.GetItemParam<object, PhoneDto>(query, new { Gsm = gsm }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            return resService;
        }

        public async Task<Response<List<PhoneDto>>> GetPhoneListByCrmIdAsync(Guid id, CompanyEnum company)
        {
            var query = String.Format(@"SELECT 
                                            uzm_customerphoneId, 
                                            uzm_customerphone.CreatedOn, 
                                            uzm_customerphone.CreatedBy, 
                                            uzm_customerphone.ModifiedOn, 
                                            uzm_customerphone.ModifiedBy, 
                                            OrganizationId, 
                                            uzm_customerphone.statecode, 
                                            uzm_customerphone.statuscode, 
                                            uzm_customerphone.ImportSequenceNumber, 
                                            uzm_customerphonenumber, 
                                            uzm_customerId, 
                                            uzm_phonetype, 
                                            uzm_createdlocationid, 
                                            uzm_modifiedbylocationid, 
                                            uzm_createdpersonid, 
                                            uzm_modifiedbypersonid, 
                                            uzm_phonepermission, 
                                            uzm_datasourceid, 
                                            uzm_releatedpermissionid, 
                                            uzm_unsubscribedate, 
                                            uzm_unsubscribe_channelid, 
                                            uzm_iyscallpermit, 
                                            uzm_iysphonepermit, 
                                            uzm_iysphonepermitdate, 
                                            uzm_iyscallpermitdate, 
                                            uzm_iyscallid, 
                                            uzm_iysphoneid, 
                                            uzm_iyssend,
											uzm_calliyssend,
	                                        createdportaluser.uzm_personno as createdpersonno,
	                                        updatedportaluser.uzm_personno as updatedpersonno
                                        FROM uzm_customerphone WITH(NOLOCK)
                                        LEFT JOIN uzm_portaluserBase createdportaluser WITH(NOLOCK)
                                        ON uzm_customerphone.uzm_createdpersonid = createdportaluser.uzm_portaluserId
                                        LEFT JOIN uzm_portaluserBase updatedportaluser WITH(NOLOCK)
                                        ON uzm_customerphone.uzm_modifiedbypersonid = updatedportaluser.uzm_portaluserId
                                            WHERE 
                                              uzm_customerphone.uzm_customerId = @ContactId 
                                              AND uzm_customerphone.statecode = 0"
            );

            var resService = await _dapperService.GetListByParamAsync<object, Domain.Entity.CRM.Contact.Phone>(query, new { ContactId = id }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = _mapper.Map<Response<List<PhoneDto>>>(resService);
            return response;
        }

        private async Task<Response<int>> UpdateContactPhoneAsync(PhoneSaveRequestDto requestDto)
        {
            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(UpdateContactPhoneAsync),
                 requestDto.Company,
                 LogTypeEnum.Request,
                 requestDto
                 );
            var queryCrm = @"UPDATE 
                              ContactBase 
                            SET 
                              mobilephone = @PhoneNumber 
                            WHERE 
                              ContactId = @CustomerCrmId 
                            SELECT 
                              @@ROWCOUNT";

            var queryOvm = string.Format(StaticConsts.Sql_Update_Ovm_ContactPhone, GeneralHelper.GetCrmDbNameByCompany(requestDto.Company));

            var responseCrm = await _dapperService.SaveQueryParam<PhoneSaveRequestDto, int>(queryCrm, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(requestDto.Company));
            var responseOvm = await _dapperService.SaveQueryParam<PhoneSaveRequestDto, int>(queryOvm, requestDto, GeneralHelper.GetOvmConnectionStringByCompany(requestDto.Company));

            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(UpdateContactPhoneAsync),
            requestDto.Company,
                LogTypeEnum.Response,
                responseCrm
                );

            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(UpdateContactPhoneAsync),
                requestDto.Company,
                LogTypeEnum.Response,
                responseOvm
                );

            return responseOvm;
        }



        #region DeleteContactPhoneControl...

        private async Task<Response<int>> Update_Crm_ContactPhoneAsync(PhoneSaveRequestDto requestDto)
        {
            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(Update_Crm_ContactPhoneAsync),
                 requestDto.Company,
                 LogTypeEnum.Request,
                 requestDto
                 );

            var queryOvm = string.Format(StaticConsts.Sql_Update_Crm_ContactPhone, GeneralHelper.GetCrmDbNameByCompany(requestDto.Company));

            var responseOvm = await _dapperService.SaveQueryParam<PhoneSaveRequestDto, int>(queryOvm, requestDto, GeneralHelper.GetOvmConnectionStringByCompany(requestDto.Company));


            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(Update_Crm_ContactPhoneAsync),
                requestDto.Company,
                LogTypeEnum.Response,
                responseOvm
                );

            return responseOvm;
        }
        private async Task<Response<int>> Update_Ovm_ContactPhoneAsync(PhoneSaveRequestDto requestDto)
        {
            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(Update_Ovm_ContactPhoneAsync),
                 requestDto.Company,
                 LogTypeEnum.Request,
                 requestDto
                 );

            var queryOvm = string.Format(StaticConsts.Sql_Update_Ovm_ContactPhone, GeneralHelper.GetCrmDbNameByCompany(requestDto.Company));

            var responseOvm = await _dapperService.SaveQueryParam<PhoneSaveRequestDto, int>(queryOvm, requestDto, GeneralHelper.GetOvmConnectionStringByCompany(requestDto.Company));

            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(Update_Ovm_ContactPhoneAsync),
                requestDto.Company,
                LogTypeEnum.Response,
                responseOvm
                );

            return responseOvm;
        }
        private async Task<Response<int>> Update_IsDeletedAsync(DeletePhoneRequestDto requestDto)
        {
            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(Update_IsDeletedAsync),
                 CompanyEnum.KD,
                 LogTypeEnum.Request,
                 requestDto
                 );

            var query = string.Format(@"-- ilgili müşteri telefonu hariç ilişkili olan müşteri telefon kayıtlarını pasife çekme ve isdelete flag'ini setleme
        UPDATE cp SET cp.uzm_isdeleted = 1, cp.statecode = 1, cp.statuscode = 2
        FROM KahveDunyasi_MSCRM..uzm_customerphone cp WITH(NOLOCK)
        WHERE cp.uzm_customerphoneId=@CustomerPhoneId 
        SELECT @@ROWCOUNT ", GeneralHelper.GetCrmDbNameByCompany(CompanyEnum.KD));

            var responseOvm = await _dapperService.SaveQueryParam<DeletePhoneRequestDto, int>(query, requestDto, GeneralHelper.GetOvmConnectionStringByCompany(CompanyEnum.KD));

            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(Update_IsDeletedAsync),
                CompanyEnum.KD,
                LogTypeEnum.Response,
                responseOvm
                );

            return responseOvm;
        }
        private async Task<Response<List<ContactByErpIdDto>>> GetContactDeletePhoneByErpIdAsync(DeletePhoneRequestDto requestDto)
        {
            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(GetContactDeletePhoneByErpIdAsync),
                 CompanyEnum.KD,
                 LogTypeEnum.Request,
                 requestDto
                 );

            var query = string.Format(@"select cnt.uzm_ErpId,cnt.MobilePhone,cnt.FullName,cnt.ContactId,cp.uzm_customerphoneId as CustomerPhoneId
                FROM KahveDunyasi_MSCRM..uzm_customerphone cp WITH(NOLOCK)
                INNER JOIN KahveDunyasi_MSCRM..contact cnt WITH(NOLOCK) ON cnt.ContactId = cp.uzm_customerId
                WHERE cp.uzm_customerphonenumber = @Gsm AND cp.statecode = 0
                AND cnt.uzm_customertype != 'P' AND cnt.uzm_customertype != 'Z' AND (cnt.uzm_ErpId != @ErpId OR cnt.uzm_ErpId IS NULL)
                order by cp.ModifiedOn desc",
                GeneralHelper.GetCrmDbNameByCompany(CompanyEnum.KD));

            var responseOvm = await _dapperService.GetListByParamAsync<DeletePhoneRequestDto, ContactByErpIdDto>(query, requestDto, GeneralHelper.GetOvmConnectionStringByCompany(CompanyEnum.KD));

            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                this.GetType().Name,
                nameof(GetContactDeletePhoneByErpIdAsync),
                CompanyEnum.KD,
                LogTypeEnum.Response,
                responseOvm
                );

            return responseOvm;
        }
        public async Task<Response<int>> DeleteContactPhoneControl(DeletePhoneRequestDto requestDto)
        {

            var result = new Response<int>();
            await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(DeleteContactPhoneControl),
                 CompanyEnum.KD,
                 LogTypeEnum.Request,
                 requestDto
                 );
            try
            {
                //Var olar ErpId haric olan dataları alıp devre dışı bırakma işlemleri sağlanıyor.
                var response = await GetContactDeletePhoneByErpIdAsync(requestDto);
                var mostRecentPhoneErpId = response.Success ? response.Data[0].uzm_ErpId : "";
                foreach (var item in response.Data)
                {
                    if (requestDto.CustomerType == "Z" & item.uzm_ErpId == mostRecentPhoneErpId)
                        continue;

                    //Update_IsDeletedAsync
                    var update_IsDeleted = await Update_IsDeletedAsync(new DeletePhoneRequestDto { CustomerPhoneId = item.CustomerPhoneId, ErpId = item.uzm_ErpId, Gsm = item.MobilePhone });

                    //Update_Ovm_ContactPhoneAsync
                    var update_Ovm_ContactPhone = await Update_Ovm_ContactPhoneAsync(new PhoneSaveRequestDto { CustomerCrmId = item.ContactId });

                    //Update_Crm_ContactPhoneAsync
                    var update_Crm_ContactPhone = await Update_Crm_ContactPhoneAsync(new PhoneSaveRequestDto { CustomerCrmId = item.ContactId });

                    if (update_IsDeleted.Success)
                    {
                        var setResult = CommonMethod.SetResponseSuccess(0);
                        if (!result.Success)
                            result = setResult;

                        result.Data += (update_IsDeleted.Data + update_Ovm_ContactPhone.Data + update_Crm_ContactPhone.Data);
                    }
                }

                await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                    this.GetType().Name,
                    nameof(DeleteContactPhoneControl),
                    CompanyEnum.KD,
                    LogTypeEnum.Response,
                    response
                    );

                return result;

            }
            catch (Exception ex)
            {
                await _logService.LogSave(Common.Enums.LogEventEnum.DbError,
                this.GetType().Name,
                nameof(DeleteContactPhoneControl),
                CompanyEnum.KD,
                LogTypeEnum.Response,
                ex.ToString()
                );
            }



            return null;
        }

        #endregion
    }
}
