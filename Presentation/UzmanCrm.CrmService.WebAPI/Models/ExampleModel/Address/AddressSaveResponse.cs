using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebAPI.Models.Address
{
    public class AddressSaveResponse
    {
        /// <summary>
        /// Crm sisteminde oluşan Guid AdresId bilgisidir. 
        /// </summary>
        public Guid? Id { get; set; } = null;

        /// <summary>
        /// Crm sisteminde oluşan Guid crmId bilgisidir. 
        /// </summary>
        public Guid? CrmId { get; set; } = null;

        /// <summary>
        /// Kayıt yeni oluştu ise Create=0 , var olan bir kayıt ise Update=1 bilgisi. 
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CreateType Type { get; set; }


    }
}