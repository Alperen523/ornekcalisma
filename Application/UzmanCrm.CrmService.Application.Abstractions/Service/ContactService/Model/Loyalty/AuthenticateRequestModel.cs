using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Loyalty
{
    public class AuthenticateRequestModel
    {
        public string Username { get; set; } = "crmapi";
        public string Password { get; set; } = "uzm_123";
    }
}
