using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoginService.Model
{
    public class ApiUserLoginResponseDto
    {

        public Guid? uzm_apiuserloginid { get; set; } = null;
        public string uzm_username { get; set; } = null;
        public string uzm_password { get; set; } = null;
        public string uzm_roles { get; set; } = null;
    }
}
