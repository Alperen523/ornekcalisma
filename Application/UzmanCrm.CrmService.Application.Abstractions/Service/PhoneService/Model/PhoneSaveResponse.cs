using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model
{
    public class PhoneSaveResponse
    {
        /// <summary>
        /// Müşteri telefonunun id bilgisi
        /// </summary>
        public Guid? Id { get; set; } = null;

        /// <summary>
        /// Müşteri crm id bilgisi
        /// </summary>
        public Guid? CustomerCrmId { get; set; } = null;

    }
}
