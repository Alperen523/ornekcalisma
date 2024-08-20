using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class LoyaltyCardInfoOnContactRequest
    {
        public Guid contactid { get; set; }
        public Guid uzm_loyaltycardid { get; set; }
        public string uzm_loyaltycardno { get; set; }
        public CardTypeEnum uzm_loyaltycardtype { get; set; }
    }
}
