using System;
using System.ComponentModel.DataAnnotations;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebAPI.Models.Contact
{
    //[Validator(typeof(SearchCustomerRequestValidator))]
    /// <summary>
    /// EcomId ye göre müşteri getirme request modeli
    /// </summary>
    public class GetCustomerByEcomIdResponse
    {
        /// <summary>
        /// Crm de oluşan Id bilgisidir.
        /// </summary>
        public Guid? CrmId { get; set; } = null;

        /// <summary>
        /// Vakko erpId bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

        /// <summary>
        /// Müşteri ad bilgisidir.
        /// </summary>
        public string FirstName { get; set; } = null;

        /// <summary>
        /// Müşteri soyad bilgisidir.
        /// </summary>
        public string LastName { get; set; } = null;

        /// <summary>
        /// Müşteri doğumtarihi bilgisidir.
        /// </summary>
        public DateTime? BirthDate { get; set; } = null;

        /// <summary>
        /// Loyalty kartno bilgisidir.
        /// </summary>
        public string CardNo { get; set; } = null;

        /// <summary>
        /// Loyalty kart tipi bilgisidir.
        /// </summary>
        public string CardType { get; set; } = null;

        /// <summary>
        /// Müşteri kayıt durumu bilgisidir.  Aktif/Pasif
        /// </summary>
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;

        /// <summary>
        /// Kaydın son değişiklik tarihi bilgisidir.
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; } = null;

        /// <summary>
        /// Müşteri Erp tipi bilgisidir.
        /// </summary>
        public string CustomerType { get; set; } = null;

        /// <summary>
        /// Cinsiyet bilgisidir.  Erkek = 1,Kadin = 2,Bilinmiyor = 3
        /// </summary>
        public GenderEnum GenderId { get; set; } = GenderEnum.Bilinmiyor;

        /// <summary>
        /// Kayıt dijitalizin(UDİ) den geçmiş ise **true** olarak değil ise **false** olmalıdır. 
        /// </summary>
        public bool? IsKvkk { get; set; } = null;

        /// <summary>
        /// Ülke kodu olmayan telefon bilgisi. Örn: "5551234567"
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Sms (Mesaj) izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? SmsPermit { get; set; } = false;

        /// <summary>
        /// Arama izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? CallPermit { get; set; } = false;

        /// <summary>
        /// Email adresi bilgisidir
        /// </summary>
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Email adresi izin bilgisidir. false=izinsiz , true=izinli
        /// </summary>
        public bool EmailPermit { get; set; } = false;

    }
}