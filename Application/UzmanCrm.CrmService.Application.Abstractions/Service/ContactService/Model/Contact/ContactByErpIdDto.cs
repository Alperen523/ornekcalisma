using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class ContactByErpIdDto 
    {
        public Guid? CustomerPhoneId { get; set; } = null;
        public Guid? ContactId { get; set; } = null;
        public string MobilePhone { get; set; } = null;
        public string EMailAddress1 { get; set; } = null;
        public string uzm_ErpId { get; set; } = null;
        public string uzm_customertype { get; set; } = null;
        public string FullName { get; set; } = null;
    }
}
