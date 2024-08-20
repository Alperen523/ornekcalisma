using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model
{
    public class ContactEmailModelDto
    {
        public string EmailAddress { get; set; }
        public bool EmailPermit { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
