using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.Address
{
    [Validator(typeof(AddressSaveRequestValidator))]
    /// <summary>
    /// Ecom Müşteri Adresi Kaydetme request modeli
    /// </summary>
    public class AddressSaveRequest
    {
        /// <summary>
        /// Erp sistemindeki benzersiz id bilgisidir.
        /// </summary>
        [Required]
        public string ErpId { get; set; } = null;

        /// <summary>
        /// Eticaret sistemindeki Ecommerce **AddressId** bilgisidir. 
        /// </summary>
        [Required]
        public string AddressId { get; set; }

        /// <summary>
        /// Crm sistemindeki ülke kodu bilgisidir.  
        /// </summary>
        [Required]
        public CountryCodeEnum CountryCode { get; set; } = CountryCodeEnum.Bilinmiyor;

        /// <summary>
        /// Crm sistemindeki il kodu bilgisi
        /// </summary>
        public string CityCode { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki ilçe kodu bilgisi
        /// </summary>
        public string DistrictCode { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki semt kodu bilgisi.
        /// </summary>
        public string NeighborhoodCode { get; set; } = null;

        /// <summary>
        /// Açık adres bilgisi
        /// </summary>
        public string AddressLine { get; set; } = null;

        /// <summary>
        /// Posta kodu bilgisi
        /// </summary>
        public string PostCode { get; set; } = null;

        /// <summary>
        /// Kaydı oluşturan ve değiştiren mağaza kodu bilgisidir.
        /// </summary>
        [Required]
        public string Location { get; set; } = null;

        /// <summary>
        /// Adres Tipi bilgisi
        /// <br/> Ev = 0,Is = 1,Fatura = 2,Tatil_Yazlik = 3,Teslimat_Adresi = 4
        /// </summary>
        [Required]
        public AddressTypeEnum AddressType { get; set; } = AddressTypeEnum.Fatura;

        /// <summary>
        /// Veri Kanalı bilgisi
        /// <br/> Bilinmiyor = 0,Magaza = 4,MobilApp = 11,ETicaret = 3,CrmPortal = 2,Crm = 1
        /// </summary>
        public ChannelEnum ChannelId { get; set; } = ChannelEnum.Bilinmiyor;

        /// <summary>
        /// Varsayılan Adres bilgisidir. true = Varsayılan, false=Varsayılan değil
        /// <br/> Varsayılan Adres **true** olarak set edilirse crm müşteri kartındaki özet adres bilgisi en güncel adres olarak güncellenir.
        /// </summary>
        [Required]
        public bool IsDefaultAddress { get; set; }

    }
}