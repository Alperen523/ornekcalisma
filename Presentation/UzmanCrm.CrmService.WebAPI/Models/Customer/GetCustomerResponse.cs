using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebAPI.Models.Customer
{
    public class GetCustomerResponse
    {
        /// <summary>
        /// Crm de oluşan Id bilgisidir.
        /// </summary>
        public Guid? CrmId { get; set; } = null;

        /// <summary>
        /// Mobil Id bilgisidir.
        /// Uygulama indirildiği anda atanan id dir. Mobil uygulama ile CRM arasındaki entegrasyon bu id üzerinden sağlanmaktadır.
        /// </summary>
        public string MobilId { get; set; } = null;

        /// <summary>
        /// Müşteri tipi bilgisidir.
        /// <br/> Personel Müşteri = 1,
        /// <br/> Gerçek Müşteri = 2,
        /// <br/> AVM Personeli = 3,
        /// <br/> Öğrenci = 4,
        /// <br/> Sağlık Çalışanı = 5,
        /// <br/> Esnaf = 6
        /// </summary>
        public CustomerTypeEnum CustomerTypeId { get; set; } = CustomerTypeEnum.Bilinmiyor;

        /// <summary>
        /// Müşteri ad bilgisidir.
        /// </summary>
        public string FirstName { get; set; } = null;

        /// <summary>
        /// Müşteri soyad bilgisidir.
        /// </summary>
        public string LastName { get; set; } = null;

        /// <summary>
        /// Müşteri cinsiyet bilgisidir.
        /// <br/> Erkek = 1,
        /// <br/> Kadin = 2,
        /// <br/> Bilinmiyor = 3
        /// </summary>
        public GenderEnum GenderId { get; set; } = GenderEnum.Bilinmiyor;

        /// <summary>
        /// Müşteri doğumtarihi bilgisidir.
        /// </summary>
        public DateTime? BirthDate { get; set; } = null;

        /// <summary>
        /// Müşteri Sadakat Aktifleştirme Tarihi
        /// </summary>
        public DateTime? LoyaltyActivationDate { get; set; } = null;

        /// <summary>
        /// Müşteri gsm bilgisidir.
        /// </summary>
        public string MobilePhone { get; set; } = null;

        /// <summary>
        /// Sms (Mesaj) izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? SmsPermit { get; set; } = false;

        /// <summary>
        /// Arama izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? CallPermit { get; set; } = false;

        /// <summary>
        /// Müşteri email bilgisidir.
        /// </summary>
        public string EmailAddress { get; set; } = null;

        /// <summary>
        /// Email adresi izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? EmailPermit { get; set; } = false;

        /// <summary>
        /// Kvkk izinli ise **true** değil ise **false** olmalıdır. 
        /// </summary>
        public bool? KvkkPermit { get; set; } = null;

        /// <summary>
        /// Mobil uygulama indirme tarihi.
        /// </summary>
        public DateTime? MobileAppDownloadDate { get; set; } = null;

        /// <summary>
        /// Müşteri ülke id bilgisidir.
        /// </summary>
        public int? CountryId { get; set; } = null;
        /// <summary>
        /// Müşteri il id bilgisidir.
        /// </summary>
        public int? CityId { get; set; } = null;
        /// <summary>
        /// Müşteri ilçe id bilgisidir.
        /// </summary>
        public int? DisctrictId { get; set; } = null;
        /// <summary>
        /// Müşteri semt id bilgisidir.
        /// </summary>
        public int? NeighborhoodId { get; set; } = null;
        /// <summary>
        /// Müşteri güncel adres bilgisidir.
        /// </summary>
        public string AdressLine { get; set; } = null;



    }
}