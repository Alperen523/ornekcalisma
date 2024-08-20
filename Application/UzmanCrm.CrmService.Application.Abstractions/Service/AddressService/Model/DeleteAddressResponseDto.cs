using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model
{
    public class DeleteAddressResponseDto
    {
        public Guid? Id { get; set; } = null;
        public Guid? CrmId { get; set; } = null;
        public string ErpId { get; set; } = null;
    }
}
