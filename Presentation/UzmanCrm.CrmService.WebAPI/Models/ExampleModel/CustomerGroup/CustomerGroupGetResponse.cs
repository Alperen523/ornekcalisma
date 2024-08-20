using System;

namespace UzmanCrm.CrmService.WebAPI.Models.CustomerGroup
{
    public class CustomerGroupGetResponse
    {
        /// <summary>
        /// Crm'de kayıtlı benzersiz müşteri grubu id bilgisidir.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Müşteri grubu isim bilgisidir.
        /// </summary>
        public string CustomerGroupName { get; set; }

        /// <summary>
        /// Müşteri grubu kodu bilgisidir.
        /// </summary>
        public string Code { get; set; }
    }
}