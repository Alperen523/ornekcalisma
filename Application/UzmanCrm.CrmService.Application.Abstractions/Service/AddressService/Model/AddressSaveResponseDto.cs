using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model
{
    public class AddressSaveResponseDto
    {

        public Guid? Id { get; set; } = null;
        public Guid? CrmId { get; set; } = null;

        [JsonConverter(typeof(StringEnumConverter))]
        public CreateType Type { get; set; }

    }
}
