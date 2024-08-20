using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Contact;

namespace UzmanCrm.CrmService.Application.Service.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IMapper _mapper;
        private readonly ICRMService _crmService;
        private readonly ILogService _logService;
        private readonly IDapperService _dapperService;
        private readonly IUserService _userService;
        private readonly IBusinessUnitService _businessService;

        public EmailService(IMapper mapper, ICRMService crmService, ILogService logService, IDapperService dapperService, IUserService userService, IBusinessUnitService businessService)
        {
            _crmService = crmService;
            _logService = logService;
            _dapperService = dapperService;
            _userService = userService;
            _businessService = businessService;
            _mapper = mapper;
        }


        public Response<EmailResponse> SendEmail(string _subject, string _body, string ToType, string[] _portalUserId = null, List<string> _toList = null, List<string> _ccList = null, object _attachment = null, string _attachtype = "", string _attachname = "")
        {
            var result = new Response<EmailResponse>();
            var toList = new Response<List<ToMailSend>>();
            try
            {
                string subject = _subject;
                string body = _body;
                string[] toids = new string[1];
                if (_portalUserId != null)
                    toids = _portalUserId;

                result.Data.Id = _crmService.SendCrmEmail(toids, "uzm_portaluser", subject, body, _toList, _ccList, _attachment, _attachtype, _attachname);
            }
            catch (Exception ex)
            {
                _logService.LogSave(LogEventEnum.DbError, this.GetType().Name, nameof(SendEmail), CompanyEnum.KD, LogTypeEnum.Response, ex);
                throw;
            }
            return result;
        }

        public async Task<Response<EmailSaveResponse>> EmailSaveAsync(EmailSaveRequestDto requestDto)
        {
            var responseModel = new Response<EmailSaveResponse>();

            //Person
            if (requestDto.PersonId is null || requestDto.PersonId == Guid.Empty)
            {
                var portalUserResponse = await _userService.GetEmployeeNumberAsync(requestDto.PersonNo, requestDto.Company);
                if (portalUserResponse.Success)
                    requestDto.PersonId = portalUserResponse.Data.uzm_employeeid;
            }

            //Store
            if (requestDto.StoreId is null || requestDto.StoreId == Guid.Empty)
            {
                var storeResponse = await _businessService.GetStoreCodeAsync(requestDto.StoreCode, requestDto.Company);
                if (storeResponse.Success)
                    requestDto.StoreId = storeResponse.Data.uzm_storeid;
            }


            var emailDto = _mapper.Map<EmailDto>(requestDto);

            var resService = await GetEmailItemAsync(requestDto);
            if (resService.Success)
            {
                emailDto.uzm_customeremailid = resService.Data.uzm_customeremailid;
                emailDto.uzm_createdbypersonid = null;
                emailDto.uzm_createdbystoreid = null;
            }

            try
            {
                var entityModel = _mapper.Map<Email>(emailDto);
                entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);

                var result = _crmService.Save<Email>(entityModel, "uzm_customeremail", "uzm_customeremail", requestDto.Company);

                await _logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                   this.GetType().Name,
                   nameof(EmailSaveAsync),
                   requestDto.Company,
                   LogTypeEnum.Response,
                   result
                   );

                responseModel.Data.Id = result.Data;
                responseModel.Data.CustomerCrmId = emailDto.uzm_contactid;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;


            }
            catch (Exception ex)
            {
                await _logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(EmailSaveAsync),
                   requestDto.Company,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<EmailSaveResponse>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.EmailSaveError + ex.ToString(), "")));

            }

            return responseModel;
        }

        private async Task<Response<EmailDto>> GetEmailItemAsync(EmailSaveRequestDto requestDto)
        {
            var query = String.Format(@"select uzm_customeremailId from uzm_customeremailBase with(nolock) where uzm_emailaddress=@EmailAddress and [uzm_contactid]=@CustomerCrmId and statecode=0");
            var resService = await _dapperService.GetItemParam<object, EmailDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(requestDto.Company)).ConfigureAwait(false);

            return resService;

        }

    }
}