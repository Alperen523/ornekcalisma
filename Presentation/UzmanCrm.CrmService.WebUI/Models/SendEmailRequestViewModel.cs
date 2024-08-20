using System;
using System.Collections.Generic;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class SendEmailRequestViewModel
    {
        public string LoyaltyCardId { get; set; }
        public string PortalUserId { get; set; }
        public string ApprovingSuperVisorId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string RequestResult { get; set; }
        public List<string> MailList { get; set; }
        public double? DiscountRate { get; set; } = null;
        public string CustomerName { get; set; } = null;
        public double? ValidDiscountRate { get; set; } = null;
        public string DemandedUserName { get; set; } = null;
        public DateTime EndDate { get; set; }
        public string Description { get; set; } = null;
        public string DemandStore { get; set; } = null;
    }
}