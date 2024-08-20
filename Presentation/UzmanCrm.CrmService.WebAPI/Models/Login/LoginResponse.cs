using System.Net.Http;

namespace UzmanCrm.CrmService.WebAPI.Models.Login
{
    /// <summary>
    /// Login cevap modeli
    /// </summary>
    public class LoginResponse
    {
        public LoginResponse()
        {
            Token = "";
            responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        public string Token { get; set; }

        public HttpResponseMessage responseMsg { get; set; }
    }

}