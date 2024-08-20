using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class SaveCustomerResponseDto
    { /// <summary>
      /// Crm sisteminde oluşan Guid crmId bilgisidir. 
      /// </summary>
        public Guid? CrmId { get; set; } = null;

        /// <summary>
        /// Kayıt yeni oluştu ise Create=0 , var olan bir kayıt ise Update=1 bilgisi. 
        /// </summary>
        public CreateType Type { get; set; }


    }
}
