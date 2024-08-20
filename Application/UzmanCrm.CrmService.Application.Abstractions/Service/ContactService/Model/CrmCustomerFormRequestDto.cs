using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class CrmCustomerFormRequestDto
    {
        public Guid CrmId { get; set; }
        public DigitalFormEnum FormType { get; set; }
        public CreateType CreateType { get; set; }
        public Guid CreatedLocation { get; set; }
        public Guid CreatedPersonId { get; set; }
        public string FormNo { get; set; }
        public CompanyEnum Company { get; set; } = CompanyEnum.KD;
        public DateTime? ApprovalDate { get; set; } = null;
        public string ApprovalNumber { get; set; } = null;
    }
}
