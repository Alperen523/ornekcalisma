using System.ComponentModel.DataAnnotations;

namespace UzmanCrm.CrmService.WebAPI.Models.Address
{
    //[Validator(typeof(AddressSaveRequestValidator))]
    /// <summary>
    /// Ecom Müşteri Adresi Silme request modeli
    /// </summary>
    public class DeleteAddressRequest
    {

        /// <summary>
        /// Eticaret sistemindeki Ecommerce **AddressId** bilgisidir. 
        /// </summary>
        [Required]
        public string AddressId { get; set; }

        /// <summary>
        /// Kaydı silen mağaza kodu bilgisidir.
        /// </summary>
        [Required]
        public string Location { get; set; } = null;

        /// <summary>
        /// Eticaret Id bilgisidir. 
        /// </summary>
        [Required]
        public string EcomId { get; set; } = null;

    }
}