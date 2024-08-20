using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model
{
    public class LoyaltyCardUpdateDto
    {
        public Guid Id { get; set; }

        public StatusType StatusEnum { get; set; } = StatusType.Aktif;

        public string CardNumber { get; set; } = null;

        public Guid? ContactId { get; set; } = null;

        public LoyaltyCardStatusCodeType? CardStatusCodeType { get; set; } = null;

        public string ErpId { get; set; } = null;

        public string MobilePhone { get; set; } = null;

        public string Email { get; set; } = null;

        public Guid? CardTypeId { get; set; } = null;

        public double? ValidEndorsement { get; set; } = null;

        public double? ValidDiscountRateVakko { get; set; } = null;
        public double? ValidDiscountRateVr { get; set; } = null;
        public double? ValidDiscountRateWcol { get; set; } = null;

        public double? AmountForUpperSegmentVakko { get; set; } = null;
        public double? AmountForUpperSegmentVr { get; set; } = null;
        public double? AmountForUpperSegmentWcol { get; set; } = null;

        public double? UpperSegmentDiscountPercentVakko { get; set; } = null;
        public double? UpperSegmentDiscountPercentVr { get; set; } = null;
        public double? UpperSegmentDiscountPercentWcol { get; set; } = null;

        public double? TurnoverEndorsement { get; set; } = null;

        public double? PeriodEndorsement { get; set; } = null;

        public double? DifferenceEndorsement { get; set; } = null;

        public Guid? CustomerEndorsementId { get; set; } = null;

    }
}
