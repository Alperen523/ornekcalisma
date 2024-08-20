using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expires_in { get; set; }
        public string Loginfo { get; set; }
    }
}