using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class PortalUserRequestViewModel
    {
        public string PortalUserId { get; set; }
        public string PortalErpId { get; set; }
        public bool IsExist { get; set; } = false;
    }
}