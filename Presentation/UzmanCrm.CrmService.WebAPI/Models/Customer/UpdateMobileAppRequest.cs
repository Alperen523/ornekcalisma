using FluentValidation.Attributes;
using System;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.Customer
{
    [Validator(typeof(SaveCustomerRequestValidator))]
    /// <summary>
    /// Müşteri MobilApp Kaydetme request modeli
    /// </summary>
    public class UpdateMobileAppRequest
    {

        public Guid? CrmId { get; set; } = null;

        /// <summary>
        /// Veri Kanalı bilgisi
        /// <br/> Bilinmiyor = 0,Magaza = 1,MobilApp = 2,ETicaret = 3,CrmPortal = 4,Crm = 5
        /// </summary>
        public ChannelEnum ChannelId { get; set; } = ChannelEnum.Bilinmiyor;


        /// <summary>
        /// Mobil Id bilgisidir.
        /// <br/>Uygulama indirildiği anda atanan id dir. Mobil uygulama ile CRM arasındaki entegrasyon bu id üzerinden sağlanmaktadır.
        /// </summary>
        public string MobileId { get; set; } = null;

        /// <summary>
        /// Mobil Uygulama indirilme tarihidir.
        /// </summary>
        public DateTime? MobileAppDownloadedDate { get; set; } = null;


    }
}