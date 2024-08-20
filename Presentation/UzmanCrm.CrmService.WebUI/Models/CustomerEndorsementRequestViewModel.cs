using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class CustomerEndorsementRequestViewModel
    {
        /// <summary>
        /// Erp sistemindeki benzersiz müşteri Id bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

        /// <summary>
        /// Müşteri Kart numarası bilgisidir.
        /// </summary>
        public string CardType { get; set; } = null;
    }
}