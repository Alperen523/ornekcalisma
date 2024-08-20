using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact
{
    public class CreateCustomerFormResponse
    {
        /// <summary>
        /// Email id bilgisi
        /// </summary>
        public Guid? Id { get; set; } = null;

        /// <summary>
        /// Müşteri crm id bilgisi
        /// </summary>
        public Guid? CustomerCrmId { get; set; } = null;

    }
}
