using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class CardApprovalStatusAndExplanationRequestViewModel
    {
        public string CardDiscountId { get; set; } = null;
        public string LoyaltyCardId { get; set; } = null;
        public CardDiscountStatusCodeType StatusCode { get; set; } = CardDiscountStatusCodeType.Draft;
        public ApprovalStatusType ApprovalStatus { get; set; } = ApprovalStatusType.Draft;
        public string ApprovalExplanation { get; set; } = null;
        public ArrivalChannelType ArrivalChannel { get; set; } = ArrivalChannelType.Portal;
        public string BusinessUnitId { get; set; } = null;
    }
}