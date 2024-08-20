using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class LoyaltyCardRequestViewModel
    {
        public string LoyaltyCardId { get; set; }
        public double ValidDiscountRate { get; set; }
    }
}