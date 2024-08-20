using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model
{
    public class EmailSaveResponse
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
