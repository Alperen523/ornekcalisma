using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Rest;
using UzmanCrm.CrmService.DAL.Config.Application.Common;

namespace UzmanCrm.CrmService.DAL.Config.Application.Rest
{
    public class RestService : IRestService
    {
        public async Task<Response<TRes>> SendRequest<TRes>(object obj, string url, string token = null,
            Method method = Method.Post, string AuthorizationType = "Bearer")
            where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                var jsonText = FormatHelper.JsonSerializeObject(obj);
                var client = new RestClient(url);

                var request = new RestRequest();
                request.Method = method;
                if (token.IsNotNullAndEmpty())
                    request.AddHeader("Authorization", $"{AuthorizationType} {token}");
                if (method != Method.Get)
                {
                    request.AddHeader("Content-Type", "application/json");
                    if (obj != null)
                        request.AddStringBody(jsonText, DataFormat.Json);
                }
                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    res.Success = true;
                    res.Message = CommonStaticConsts.Message.Success;
                    res.Data = JsonConvert.DeserializeObject<TRes>(response.Content);
                }
                else
                {
                    res.Success = false;
                    res.Message = response.ErrorMessage + response.Content;
                }
            }
            catch (Exception ex)
            {
                CommonMethod.SetError(res, ex.Message);
            }

            return res;
        }
    }
}

