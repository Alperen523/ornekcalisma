using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.WageScaleService.Model
{
    public class CreateCustomerWageScaleRequestDto
    {
        // Müşteri Sadakat Kartı
        public Guid? LoyaltyCardId { get; set; } = null;

        // Aktiflik Durumu
        public bool ActivityStatus { get; set; } = true;

        // Müşteri Ciro Id
        public Guid? EndorsementId { get; set; } = null;

        // İstisna İndirim Oranı
        public double? CardDiscount_DiscountRate { get; set; } = null;

        // Güncel Cıro
        public double? PeriodEndorsement { get; set; } = null;

        // Devir Cıro
        public double? TurnoverEndorsement { get; set; } = null;

        // Vakko İndirim Barem Detayı
        public Guid? WageScaleIdVakko { get; set; } = null;

        // Vakkorama İndirim Barem Detayı
        public Guid? WageScaleIdVr { get; set; } = null;

        // W Collection İndirim Barem Detayı
        public Guid? WageScaleIdWcol { get; set; } = null;

        // Vakko Geçerli İndirim Oranı
        public double? ValidDiscountRateVakko { get; set; } = null;

        // Vakkorama Geçerli İndirim Oranı
        public double? ValidDiscountRateVr { get; set; } = null;

        // WColl Geçerli İndirim Oranı
        public double? ValidDiscountRateWcol { get; set; } = null;
    }
}
