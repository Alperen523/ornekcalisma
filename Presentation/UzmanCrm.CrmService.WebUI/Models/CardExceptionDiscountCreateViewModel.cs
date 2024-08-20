using System;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class CardExceptionDiscountCreateViewModel
    {
        public Guid? LoyaltyCardId { get; set; }
        public Guid? CardClassSegmentId { get; set; }
        public Guid? CustomerGroupId { get; set; }
        public Guid? DemandedUserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DemandDate { get; set; }
        public decimal? DiscountRate { get; set; }
        public string Description { get; set; }
        public string ApprovedByUserId { get; set; }
    }
}