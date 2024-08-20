using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model
{
    public class ContactPhoneModelDto
    {
        public string PhoneNumber { get; set; }
        public bool? SmsPermit { get; set; } = false;
        public bool? CallPermit { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedPerson { get; set; }
        public string UpdatedPerson { get; set; }
        public ChannelEnum Channel { get; set; }
        public PhoneTypeEnum Type { get; set; }
    }
}
