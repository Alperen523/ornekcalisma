using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model
{
    public class PhoneSaveRequestDto : BaseRequestDto
    {
        public Guid? CustomerCrmId { get; set; } = null;
        public string PhoneNumber { get; set; } = null;
        public bool? SmsPermit { get; set; } = null;
        public bool? CallPermit { get; set; } = null;
        public Guid? ReleatedPermissionId { get; set; } = null;

    }
}
