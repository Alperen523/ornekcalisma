using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact
{

    public class GetCustomerResponseDto
    {

        public Guid? CrmId { get; set; } = null;
        public string MobilId { get; set; } = null;
        public CustomerTypeEnum CustomerTypeId { get; set; } = CustomerTypeEnum.Bilinmiyor;
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public GenderEnum GenderId { get; set; } = GenderEnum.Bilinmiyor;
        public DateTime? BirthDate { get; set; } = null;
        public DateTime? LoyaltyActivationDate { get; set; } = null;
        public string MobilePhone { get; set; } = null;
        public bool? SmsPermit { get; set; } = false;
        public bool? CallPermit { get; set; } = false;
        public string EmailAddress { get; set; } = null;
        public bool? EmailPermit { get; set; } = false;
        public bool? KvkkPermit { get; set; } = null;
        public DateTime? MobileAppDownloadDate { get; set; } = null;
        public int? CountryId { get; set; } = null;
        public int? CityId { get; set; } = null;
        public int? DisctrictId { get; set; } = null;
        public int? NeighborhoodId { get; set; } = null;
        public string AdressLine { get; set; } = null;



    }
}
