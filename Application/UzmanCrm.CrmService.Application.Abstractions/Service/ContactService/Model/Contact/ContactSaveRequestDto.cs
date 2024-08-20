using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class ContactSaveRequestDto : BaseRequestDto
    {
        public ContactSaveRequestDto()
        {
            Email = new Email();
        }
        public bool? IsKvkk { get; set; } = null;
        public Guid? CrmId { get; set; } = null;
        public string IdentificationNumber { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public GenderEnum GenderId { get; set; } = 0;
        public ChannelEnum ChannelId { get; set; } = 0;
        public DigitalFormEnum DigitalFormTypeId { get; set; }
        public string DoubleOptinCode { get; set; } = null;
        public string CountryPhoneCode { get; set; } = null;
        public string PersonNo { get; set; } = null;
        public string Location { get; set; } = null;
        public OrganizationEnum OrganizationId { get; set; }
        public Email Email { get; set; } = null;
        public CompanyEnum Company { get; set; }
        public string ErpId { get; set; }
        public string EcomId { get; set; } = null;
        public string CustomerNo { get; set; } = null;
        public string CustomerType { get; set; } = null;
        public string CardNo { get; set; } = null;
        public string CardType { get; set; } = null;
    }
    public class Email
    {
        public string EmailAddress { get; set; } = null;
        public bool? EmailPermit { get; set; } = null;
    }


}

