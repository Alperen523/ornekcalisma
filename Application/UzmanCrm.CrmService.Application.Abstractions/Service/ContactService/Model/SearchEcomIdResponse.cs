using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class SearchEcomIdResponse
    {

        public Guid? Id { get; set; } = null;

        public Guid? CrmId { get; set; } = null;

        public Guid? DatasourceId { get; set; } = null;

        public string CustomerExternalId { get; set; } = null; // Müşteri Kaynak Id  - EticaretId

        public string Phone { get; set; } = null;

        public string Email { get; set; } = null;
    }
}
