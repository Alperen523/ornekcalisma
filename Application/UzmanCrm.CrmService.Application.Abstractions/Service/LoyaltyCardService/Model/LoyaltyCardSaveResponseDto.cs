using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model
{
    public class LoyaltyCardSaveResponseDto
    {
        public Guid Id { get; set; }

        public Guid? CrmId { get; set; } = null;

        public string CardNo { get; set; } = null;
    }
}
