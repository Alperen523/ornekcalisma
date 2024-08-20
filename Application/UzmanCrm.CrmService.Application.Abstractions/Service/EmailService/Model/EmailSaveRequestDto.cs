using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model
{
    public class EmailSaveRequestDto : BaseRequestDto
    {
        public string CustomerCrmId { get; set; }
        public string EmailAddress { get; set; } = null;
        public bool? EmailPermission { get; set; } = null;
        public ChannelEnum EmailOptinChannelId { get; set; } = ChannelEnum.Bilinmiyor;
        public DateTime? EmailOptinDate { get; set; } = null;
        public ChannelEnum EmailOptoutChannelId { get; set; } = ChannelEnum.Bilinmiyor;
        public DateTime? EmailOptoutDate { get; set; } = null;
        public bool? EmailIysSendStatus { get; set; } = true;




    }
}
