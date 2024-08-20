using FluentValidation.Attributes;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard
{
    [Validator(typeof(LoyaltyCardGetRequestValidator))]
    /// <summary>
    /// Sadakat Kartı istek modeli
    /// </summary>
    public class LoyaltyCardGetRequest
    {
        /// <summary>
        /// Crm sistemindeki kart numarası bilgisidir.
        /// </summary>
        public string CardNo { get; set; } = null;

        /// <summary>
        /// Erp sistemindeki benzersiz müşteri Id bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

        /// <summary>
        /// İlgili müşterinin hangi marka için ciro-indirim bilgisi istenildiği bilgisidir<br/>
        ///     Unknown = 0,<br/>
        ///     V = 1, // Vakko <br/>
        ///     R = 2, // Vakkorama <br/>
        ///     W = 3 // WCollection
        ///     P = 4 // Personel
        /// </summary>
        public CardTypeEnum ChannelType { get; set; }
    }
}