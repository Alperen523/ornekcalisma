using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class CardApprovalEndDateRequestViewModel
    {
        public string CardDiscountId { get; set; } = null;
        public DateTime EndDate { get; set; }
        public double? DiscountRate { get; set; } = null;
    }
}