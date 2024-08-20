using System;
using UzmanCrm.CrmService.Common.Enums;
namespace UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard
{
    public class LoyaltyCardGetResponse
    {
        /// <summary>
        /// Crm'de bulunan benzersiz sadakat kart id bilgisidir.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Sadakat kart numarası bilgisidir.
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Sadakat kart sahibi müşterinin ad-soyad bilgisidir.
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// Sadakat kart tipi bilgisidir.
        /// </summary>
        public string CardTypeDefinitionName { get; set; }
        /// <summary>
        /// Kart sahibi müşterinin Erp'de kayıtlı benzersiz id bilgisidir.
        /// </summary>
        public string ErpId { get; set; }
        /// <summary>
        /// Kart durumu bilgisidir. Enum değer alır. Karşılıkları aşağıdaki gibidir:
        /// <br/>InUse = 1,// Kullanımda
        /// <br/>Blocked = 2,// Bloke Edildi
        /// <br/>Canceled = 3,// İptal Edildi <br/>
        /// </summary>
        public LoyaltyCardStatusCodeType CardStatus { get; set; }
        /// <summary>
        /// Dönem ciro bilgisidir.
        /// </summary>
        public double PeriodEndorsement { get; set; }
        /// <summary>
        /// Devir ciro bilgisidir.
        /// </summary>
        public double TurnoverEndorsement { get; set; }
        /// <summary>
        /// Sadakat kart kaydı aktiflik/pasiflik durum bilgisidir.
        /// <br/>A:Aktif - P:Pasif
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Sadakat kart kaydının güncellenme tarihi bilgisidir.
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// Kartın oluşturulduğu mağaza bilgisidir.
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// Sadakat kart kaydının oluşturulma tarihi bilgisidir.
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Sadakat kart geçerli indirim oranı bilgisidir.
        /// </summary>
        public double ValidDiscountRate { get; set; }
    }
}