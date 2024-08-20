using Swashbuckle.Examples;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response
{
    public class TokenResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<TokenResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new TokenResponse
            {
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6Ik",
                Expires_in = System.DateTime.Now,
                Loginfo = "196 Milliseconds"


            };

            return resp;

        }


    }
}

