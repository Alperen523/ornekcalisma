using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class LoginRequest
    {
        /// <summary>
        /// Sistem yöneticisi tarafından verilen api kullanıcı adı bilgisi. 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Sistem yöneticisi tarafından verilen api parola bilgisi. 
        /// </summary>
        public string Password { get; set; }
    }
}