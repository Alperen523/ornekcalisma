using FluentValidation.Attributes;
using System;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.Customer
{
    [Validator(typeof(SaveCustomerRequestValidator))]
    /// <summary>
    /// Müşteri Kaydetme request modeli
    /// </summary>
    public class SaveCustomerRequest
    {

        public Guid? CrmId { get; set; } = null;
        /// <summary>
        /// Müşteri adı bilgisidir
        /// </summary>
        public string FirstName { get; set; } = null;

        /// <summary>
        /// Müşteri soyad bilgisidir
        /// </summary>
        public string LastName { get; set; } = null;

        /// <summary>
        /// Müşteri Doğum tarihi bilgisidir. "2000-05-25"
        /// </summary>
        public DateTime? BirthDate { get; set; } = null;

        /// <summary>
        /// Cinsiyet bilgisidir.  Erkek = 1,Kadin = 2,Bilinmiyor = 3
        /// </summary>
        public GenderEnum GenderId { get; set; } = GenderEnum.Bilinmiyor;

        /// <summary>
        /// Veri Kanalı bilgisi
        /// <br/> Bilinmiyor = 0,Magaza = 1,MobilApp = 2,ETicaret = 3,CrmPortal = 4,Crm = 5
        /// </summary>
        public ChannelEnum ChannelId { get; set; } = ChannelEnum.Bilinmiyor;

        /// <summary>
        /// Kvkk izinli ise **true** değil ise **false** olmalıdır. 
        /// </summary>
        public bool? KvkkPermit { get; set; } = null;

        /// <summary>
        /// Email adresi bilgisi.
        /// </summary>
        public string EmailAddress { get; set; } = null;

        /// <summary>
        /// Email izin tarihi bilgisi.
        /// </summary>
        public DateTime? EmailPermitDate { get; set; } = null;

        /// <summary>
        /// Email adresi izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? EmailPermit { get; set; } = false;

        /// <summary>
        /// 10 haneli telefon bilgisi "5551234567"
        /// </summary>
        public string MobilePhone { get; set; } = null;

        /// <summary>
        /// Sms (Mesaj) izin tarihi bilgisi.
        /// </summary>
        public DateTime? SmsPermitDate { get; set; } = null;

        /// <summary>
        /// Sms (Mesaj) izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? SmsPermit { get; set; } = false;

        /// <summary>
        /// Arama izin tarihi bilgisi.
        /// </summary>
        public DateTime? CallPermitDate { get; set; } = null;

        /// <summary>
        /// Arama izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? CallPermit { get; set; } = false;

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
        /// Mobil Id bilgisidir.
        /// <br/>Uygulama indirildiği anda atanan id dir. Mobil uygulama ile CRM arasındaki entegrasyon bu id üzerinden sağlanmaktadır.
        /// </summary>
        public string MobileId { get; set; } = null;

        /// <summary>
        /// Mobil Uygulama indirilme tarihidir.
        /// </summary>
        public DateTime? MobileAppDownloadedDate { get; set; } = null;


        /// <summary>
        /// Kaydı oluşturan ve değiştiren personel/kullanıcı/sicilno bilgisidir.
        /// </summary>
        public string PersonNo { get; set; } = null;

        /// <summary>
        /// Kaydı oluşturan ve değiştiren mağaza kodu bilgisidir.
        /// </summary>
        public string StoreCode { get; set; } = null;
    }
}