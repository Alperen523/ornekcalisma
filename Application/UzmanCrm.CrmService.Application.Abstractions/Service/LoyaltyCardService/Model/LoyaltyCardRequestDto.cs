using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model
{
    /// <summary>
    /// Sadakat Kartı istek modeli
    /// </summary>
    public class LoyaltyCardRequestDto
    {
        public string ErpId { get; set; } = null;

        public CardTypeEnum CardType { get; set; } = CardTypeEnum.Unknown;

        public StatusType StatusEnum { get; set; } = StatusType.Aktif;

        public Guid? CrmId { get; set; } = null;

        public LoyaltyCardStatusCodeType CardStatusCodeType { get; set; } = LoyaltyCardStatusCodeType.InUse;

        public string StoreCode { get; set; } = null;
        //public string PersonelId { get; set; } = null;
    }
}


