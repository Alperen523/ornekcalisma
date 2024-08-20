using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model
{
    public class CardApprovalStatusAndExplanationRequestDto
    {
        public Guid? CardDiscountId { get; set; } = null;
        public Guid? LoyaltyCardId { get; set; } = null;
        public CardDiscountStatusCodeType? StatusCode { get; set; } = null;
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;
        public ApprovalStatusType? ApprovalStatus { get; set; } = null;
        public string ApprovalExplanation { get; set; } = null;
        public ArrivalChannelType? ArrivalChannel { get; set; } = null;
        public Guid? BusinessUnitId { get; set; } = null;
    }
}
