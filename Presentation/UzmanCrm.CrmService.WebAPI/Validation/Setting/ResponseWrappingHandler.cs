using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common;
using static UzmanCrm.CrmService.Common.ErrorStaticConsts;

namespace UzmanCrm.CrmService.WebAPI.Validation.Setting
{
    public class ResponseWrappingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (request.RequestUri.AbsolutePath.Contains("swagger"))
            {
                return response;
            }

            return BuildApiResponse(request, response);
        }

        private HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            var responseString = "";
            if (response.Content != null)
                responseString = response.Content.ReadAsStringAsync().Result.ToString();

            if (responseString.Contains("\"Data\""))
                return response;


            object content;
            //var resp = new Response<object>();
            var resp = new ResponseError<object>();
            var modelStateErrors = new ErrorModel();
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                resp.Message = "Validation Error";
                resp.Success = false;

                var error = content as HttpError;
                if (error != null)
                {
                    content = null;

                    if (error.ModelState != null)
                    {
                        var httpErrorObject = response.Content.ReadAsStringAsync().Result;

                        var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };

                        var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

                        var modelStateValues = deserializedErrorObject.ModelState.Select(kvp => new KeyValuePair<string, string>(kvp.Key, string.Join(". ", kvp.Value)));

                        if (modelStateValues != null)
                        {
                            modelStateErrors.Description = modelStateValues.FirstOrDefault().Value;
                            modelStateErrors.ErrorCode = GeneralErrorStaticConsts.V001;
                        }
                    }
                    else if (responseString.Contains("An error has occurred"))
                    {
                        resp.Message = responseString;
                        modelStateErrors.Description = CommonStaticConsts.Message.SystemError;
                        modelStateErrors.ErrorCode = GeneralErrorStaticConsts.V004;
                    }
                }
            }

            resp.Error = modelStateErrors;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                resp = new ResponseError<object>
                {
                    Error = new ErrorModel
                    {
                        Description = CommonStaticConsts.Message.UnauthorizedDesc,
                        ErrorCode = LoginErrorStaticConsts.L002,
                        StatusCode = System.Net.HttpStatusCode.Unauthorized
                    },
                    Message = CommonStaticConsts.Message.Unauthorized,
                    Success = false
                };

            }

            var newResponse = request.CreateResponse(response.StatusCode, resp);

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
}