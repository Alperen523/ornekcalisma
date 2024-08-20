using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoginService.Model
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expires_in { get; set; }
        public string Loginfo { get; set; }
    }
}
