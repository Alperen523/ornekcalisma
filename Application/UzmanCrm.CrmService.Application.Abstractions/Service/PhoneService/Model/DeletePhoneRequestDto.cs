using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model
{
    public class DeletePhoneRequestDto
    {
        public Guid? CustomerPhoneId { get; set; }
        public string ErpId { get; set; }
        public string Gsm { get; set; }
        public string CustomerType { get; set; }
    }
}
