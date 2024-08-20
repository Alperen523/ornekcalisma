using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class GetCustomerRequestDto
    {
        public string FirstName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string MobilePhone { get; set; } = null;
        public DateTime? BirthDate { get; set; } = null;
        public string EmailAddress { get; set; } = null;

    }
}
