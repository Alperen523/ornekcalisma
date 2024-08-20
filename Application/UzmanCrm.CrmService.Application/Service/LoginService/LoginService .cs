using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.RedisService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Login;

namespace UzmanCrm.CrmService.Application.Service.LoginService
{
    public class LoginService : ILoginService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly IRedisService redisService;

        public LoginService(IMapper mapper,
            IDapperService dapperService,
            ILogService logService,
            IRedisService redisService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
            this.redisService = redisService;

        }

        #region Login

        /// <summary>
        /// Notify url create from notify url post request
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public async Task<Response<TokenResponse>> Authenticate(ApiUserLoginRequestDto model)
        {
            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                  this.GetType().Name,
                  nameof(Authenticate),
                  model.Company,
                  LogTypeEnum.Request,
                  model
                  );

            Response<TokenResponse> resp = new Response<TokenResponse>();

            // Not: Yeni api user eklenirse redis'te bulunan ApiUserLoginList key bilgili kayıt silinir ve sonraki requestte güncel haliyle redis'te oluşması sağlanır.
            var key = $"ApiUserLoginList";
            var resultLoginList = redisService.GetItem<List<ApiUserLoginResponseDto>>(key);

            if (resultLoginList == null)
            {
                var query = String.Format(@"SELECT uzm_apiuserloginId,uzm_username,uzm_password,uzm_roles = STUFF((SELECT ',' + (uzm_rolecode) 
                    from uzm_apiuserloginrole r
					where r.uzm_apiuserloginid=a.uzm_apiuserloginId
                    order by r.uzm_apiuserloginroleId
                        FOR XML PATH(''), TYPE
                        ).value('.', 'NVARCHAR(MAX)') 
                    ,1,1,'')
		         FROM uzm_apiuserloginBase a WITH(NOLOCK)
		         WHERE StateCode = 0");

                var resultLoginListRes = await dapperService.GetListByParamAsync<ApiUserLoginRequestDto, ApiUserLoginResponseDto>(query, model, GeneralHelper.GetCrmConnectionStringByCompany(model.Company));
                if (resultLoginListRes.Success)
                {
                    resultLoginList = resultLoginListRes.Data;
                    redisService.SetItem(resultLoginList, key, 24, 24, false);
                }
            }

            var resultLogin = resultLoginList.Where(x => x.uzm_username == model.Username & x.uzm_password == model.Password).FirstOrDefault();

            await logService.LogSave(Common.Enums.LogEventEnum.FileInfoLog,
                 this.GetType().Name,
                 nameof(Authenticate),
                 model.Company,
                 LogTypeEnum.Response,
                 resultLogin
                 );

            if (resultLogin != null)
            {
                //var currentAuth = GetCurrentUserAuthanticate(model); bu bölüm token kontrolü içindi şuan kapatıldı.
                //if (currentAuth.Success)
                //    return currentAuth;

                var roleList = resultLogin.uzm_roles.Replace("[-1,", "").Replace(",-1]", "").Split(',').ToList();
                var rol = new ApiUserLoginRequestDto();
                foreach (var role in roleList)
                {
                    rol.Roles.Add(new RoleDto(role));
                }

                var token = JwtHelper.CreateToken(resultLogin.uzm_username, rol.Roles); //standard,admin    --userInfo.Data.Roles
                if (token != null)
                {
                    resp.Data = token;
                    resp.Success = true;
                    resp.Message = Common.CommonStaticConsts.Message.Success;
                }
            }
            else
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<TokenResponse>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.ApiUserError, ErrorStaticConsts.LoginErrorStaticConsts.L001)));

            }


            return resp;
        }

        public Response<TokenResponse> GetCurrentUserAuthanticate(ApiUserLoginRequestDto model)
        {
            Response<TokenResponse> response = new Response<TokenResponse>();

            var headers = HttpContext.Current.Request.Headers;

            if (headers != null)
            {
                var tokenHeader = headers.GetValues("Authorization");

                if (tokenHeader != null)
                {
                    var token = tokenHeader.FirstOrDefault();

                    if (string.IsNullOrEmpty(token))
                    {
                        return response;
                    }

                    response.Data.Token = token;

                    var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        var username = identity.FindFirst(ClaimTypes.Name);
                        var exp = identity.FindFirst("exp");
                        var nbf = identity.FindFirst("nbf");

                        if (username == null || exp == null || nbf == null)
                            return response;

                        if (username.Value != model.Username)
                            return response;

                        if (string.IsNullOrEmpty(exp.Value))
                            return response;

                        var expDatetime = FormatHelper.EpochTimeToDateTime(exp.Value);
                        var nbfDatetime = FormatHelper.EpochTimeToDateTime(nbf.Value);

                        if (expDatetime == null || nbfDatetime == null)
                            return response;

                        if (DateTime.Now > expDatetime && DateTime.Now < nbfDatetime)
                            return response;

                        response.Data.Expires_in = expDatetime.Value;
                        response.Success = true;
                        response.Message = CommonStaticConsts.Message.Success;
                    }

                }
            }

            return response;
        }

        #endregion Login
    }
}
