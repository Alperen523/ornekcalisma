using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebAPI.Models.Contact
{
    //[Validator(typeof(SearchCustomerRequestValidator))]
    /// <summary>
    /// EcomId ye göre müşteri getirme request modeli
    /// </summary>
    public class ParametersCustomerByEcomIdRequest
    {
        /// <summary>
        /// Eticaret Id bilgisidir.
        /// </summary>
        [DataMember(Name = "EcomId")]
        [Display(Name = "EcomId")]
        public string EcomId { get; set; } = null;

        /// <summary>
        /// Veri kanallı bilgisidir.
        /// </summary>
        public EcomChannelTypeEnum ChannelId { get; set; }

    }
}