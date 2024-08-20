using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class CardExceptionContactRequestDto
    {
        /// <summary>
        /// Crm sistemindeki benzersiz müşteri Id bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki kart numarası bilgisidir.
        /// </summary>
        public string CardNo { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki müşteri telefon numarası bilgisidir.
        /// </summary>
        public string MobilePhone { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki müşteri email adres bilgisidir.
        /// </summary>
        public string EmailAddress1 { get; set; } = null;
    }
}
