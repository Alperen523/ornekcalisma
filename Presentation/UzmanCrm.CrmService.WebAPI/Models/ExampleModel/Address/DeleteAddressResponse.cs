using System;

namespace UzmanCrm.CrmService.WebAPI.Models.Address
{
    public class DeleteAddressResponse
    {
        /// <summary>
        /// Crm sisteminde oluşan Guid AdresId bilgisidir. 
        /// </summary>
        public Guid? Id { get; set; } = null;

        /// <summary>
        /// Crm sisteminde oluşan Guid crmId bilgisidir. 
        /// </summary>
        public Guid? CrmId { get; set; } = null;

        /// <summary>
        /// Erp sistemindeki benzersiz id bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

    }
}