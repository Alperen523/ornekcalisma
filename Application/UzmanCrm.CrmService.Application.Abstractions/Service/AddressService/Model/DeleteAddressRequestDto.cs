using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model
{
    public class DeleteAddressRequestDto
    {
        public string AddressId { get; set; }
        public string Location { get; set; } = null;
        public string EcomId { get; set; } = null;

        public Guid? CustomerCrmId { get; set; } = null;
        public Guid? AddressCrmId { get; set; } = null;
        public Guid? LocationId { get; set; } = null;
        public Guid? PersonId { get; set; } = null;
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;
    }
}
