using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model
{
    public class CardExceptionDiscountRequestDto
    {
        public Guid? CardDiscountId { get; set; } = null;

        public Guid? LoyaltyCardId { get; set; } = null;

        public Guid? CardClassSegmentId { get; set; } = null;

        public Guid? CustomerGroupId { get; set; } = null;

        public Guid? DemandedUserId { get; set; } = null;

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public DateTime? DemandDate { get; set; } = DateTime.Now;

        public decimal? DiscountRate { get; set; } = null;

        public string Description { get; set; } = null;
        
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;

        public CardDiscountStatusCodeType StatusCode { get; set; } = CardDiscountStatusCodeType.Draft;

        public ApprovalStatusType ApprovalStatus { get; set; } = ApprovalStatusType.Draft;

        public Guid? ApprovedByUserId { get; set; } = null;

        public Guid? DemandStore { get; set; } = null;

        public int? ArrivalChannel { get; set; } = null;

        public string ApprovalExplanation { get; set; } = null;

    }
}
