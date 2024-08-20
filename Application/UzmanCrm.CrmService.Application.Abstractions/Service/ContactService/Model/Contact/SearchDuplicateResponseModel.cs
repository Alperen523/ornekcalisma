using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact
{
    public class SearchDuplicateResponseModel
    {
        public Guid CrmId { get; set; }
        public string CustomerType { get; set; }
        public string DuplicateCode { get; set; }
    }
}
