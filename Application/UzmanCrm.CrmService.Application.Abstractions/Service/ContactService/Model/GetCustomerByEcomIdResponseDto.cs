using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class GetCustomerByEcomIdResponseDto
    {

        public Guid? CrmId { get; set; } = null;
        public string ErpId { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public string CardNo { get; set; } = null;
        public string CardType { get; set; } = null;
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;
        public DateTime? LastUpdatedDate { get; set; } = null;
        public string CustomerType { get; set; } = null;
        public GenderEnum GenderId { get; set; } = GenderEnum.Bilinmiyor;
        public bool? IsKvkk { get; set; } = null;
        public string PhoneNumber { get; set; }
        public bool? SmsPermit { get; set; } = false;
        public bool? CallPermit { get; set; } = false;
        public string EmailAddress { get; set; }
        public bool EmailPermit { get; set; } = false;
        public string CardTypeWcol { get; set; } = null;
        public string CardNoWcol { get; set; } = null;

    }
}
