using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class ContactByErpAndCardIdDto
    {
        public Guid? contactid { get; set; } = null;
        public string uzm_erpid { get; set; } = null;
        public Guid? uzm_loyaltycardid { get; set; } = null;
    }
}
