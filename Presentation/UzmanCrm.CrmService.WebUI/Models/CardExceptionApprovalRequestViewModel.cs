using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class CardExceptionApprovalRequestViewModel
    {
        public string LoyaltyCardId { get; set; }
        public int? StatusCode { get; set; }
        public int? ApprovalStatus { get; set; }
        public string DemandStore { get; set; }
    }
}