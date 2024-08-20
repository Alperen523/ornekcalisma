using FluentValidation.Attributes;
using System;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.CardExceptionDiscount
{
    [Validator(typeof(CardExceptionDiscountRequestValidator))]
    /// <summary>
    /// Kart İstisna İndirimi istek modeli
    /// </summary>
    public class CardExceptionDiscountRequest
    {
        /// <summary>
        /// Crm sistemindeki benzersiz Müşteri Loyalty Kartı id bilgisidir
        /// <br/>Loyalty Card id bilgisi için "api/loyalty/get-loyalty-card" endpointi kullanılabilir.
        /// </summary>
        public Guid? LoyaltyCardId { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki benzersiz Kart Sınıfı Segmenti id bilgisidir.
        /// </summary>
        public Guid? CardClassSegmentId { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki benzersiz Müşteri Grubu id bilgisidir.
        /// </summary>
        public Guid? CustomerGroupId { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki benzersiz Talep Eden id bilgisidir. Aktif Portal Kullanıcısı kayıtlarından seçilir
        /// </summary>
        public Guid? DemandedUserId { get; set; } = null;

        /// <summary>
        /// Kart istisna indiriminin başladığı tarih bilgisidir.
        /// </summary>
        public DateTime? StartDate { get; set; } = null;

        /// <summary>
        /// Kart istisna indiriminin biteceği tarih bilgisidir.
        /// </summary>
        public DateTime? EndDate { get; set; } = null;

        /// <summary>
        /// Kart istisna indiriminin talep edildiği tarih bilgisidir.
        /// </summary>
        public DateTime? DemandDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Kart istisna indiriminde kullanılacak indirim yüzdesi bilgisidir
        /// </summary>
        public decimal? DiscountRate { get; set; } = null;

        /// <summary>
        /// İsteğe bağlı eklenecek detay bilgisidir
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// İstisna kaydı kullanım durumu bilgisidir. Enum değer alır. Karşılıkları aşağıdaki gibidir:
        /// <br/>Draft = 0, // Taslak
        /// <br/>InUse = 1,// Kullanımda
        /// <br/>Blocked = 2,// Bloke Edildi
        /// <br/>Expired = 3, // Süresi Doldu
        /// <br/>Canceled = 4, // İptal Edildi <br/>
        /// </summary>
        public CardDiscountStatusCodeType StatusCode { get; set; } = CardDiscountStatusCodeType.Draft;

        /// <summary>
        /// İstisna kaydı onay durumu bilgisidir. Enum değer alır. Karşılıkları aşağıdaki gibidir:
        /// <br/>Draft = 1, // Taslak
        /// <br/>Denied = 2, // Reddedildi
        /// <br/>Approved = 3, // Onaylandı
        /// <br/>WaitingForApproval = 4, // Onay Beklemede <br/>
        /// </summary>
        public ApprovalStatusType ApprovalStatus { get; set; } = ApprovalStatusType.Draft;

        /// <summary>
        /// Crm sistemindeki benzersiz Onaylayan id bilgisidir. Aktif Portal Kullanıcısı kayıtlarından seçilir
        /// </summary>
        public Guid? ApprovedByUserId { get; set; } = null;
    }
}