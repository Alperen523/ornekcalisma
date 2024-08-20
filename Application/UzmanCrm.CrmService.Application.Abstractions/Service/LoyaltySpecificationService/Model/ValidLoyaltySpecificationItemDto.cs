using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltySpecificationService.Model
{
    public class ValidLoyaltySpecificationItemDto
    {
        public Guid? uzm_loyaltyspecificationId { get; set; } = null;
        public bool? uzm_discountfixingflag { get; set; } = null;
        public DateTime? uzm_wagescalestartcalculationdate { get; set; } = null;
        public string uzm_name { get; set; } = null;
    }
}
