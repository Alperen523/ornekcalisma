using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard
{
    /// <summary>
    /// Sadakat Kartı istek modeli
    /// </summary>
    public class LoyaltyCardRequest
    {
        /// <summary>
        /// Erp sistemindeki benzersiz id bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

        /// <summary>
        /// Kart tipi bilgisidir. (Vakko, Vakkorama, W Collection gibi)
        /// </summary>
        public CardTypeEnum CardType { get; set; } = CardTypeEnum.Unknown;

        /// <summary>
        /// Mağaza kodu bilgisidir.
        /// </summary>
        public string StoreCode { get; set; } = null;

        //public string PersonelId { get; set; } = null;
        //public string CrmUserId { get; set; } = null;
    }
}