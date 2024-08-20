using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Loyalty
{
    public class AuthenticateResponseModel
    {
        public bool Success { get; set; }  
        public string Message { get; set; } = null;
        public Data Data { get; set; } = null;
        public object Error { get; set; } = null;
    }
    public class Data
    {
        public string Token { get; set; }
        public DateTime Expires_in { get; set; }
        public string Loginfo { get; set; }
    }

}
