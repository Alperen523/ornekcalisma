using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class SaveCustomerRequestDto : BaseRequestDto
    {
        public Guid? CrmId { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public GenderEnum GenderId { get; set; } = GenderEnum.Bilinmiyor;
        public ChannelEnum ChannelId { get; set; } = ChannelEnum.Bilinmiyor;
        public bool? KvkkPermit { get; set; } = null;
        public string EmailAddress { get; set; } = null;
        public DateTime? EmailPermitDate { get; set; } = null;
        public bool? EmailPermit { get; set; } = false;
        public string MobilePhone { get; set; } = null;
        public DateTime? SmsPermitDate { get; set; } = null;
        public bool? SmsPermit { get; set; } = false;
        public DateTime? CallPermitDate { get; set; } = null;
        public bool? CallPermit { get; set; } = false;
        public CustomerTypeEnum CustomerTypeId { get; set; } = CustomerTypeEnum.Bilinmiyor;
        public string MobileId { get; set; } = null;
        public DateTime? MobileAppDownloadedDate { get; set; } = null;

    }
}
