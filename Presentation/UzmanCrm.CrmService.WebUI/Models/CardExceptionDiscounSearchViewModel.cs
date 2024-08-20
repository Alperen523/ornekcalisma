using System;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class CardExceptionDiscounSearchViewModel
    {
        /// <summary>
        /// Crm sistemindeki kart numarası bilgisidir.
        /// </summary>
        public string CardNo { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki benzersiz müşteri Id bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

        /// <summary>
        /// Crm sistemindeki channel type bilgisidir.
        /// </summary>
        public int ChannelType { get; set; }
        /// <summary>
        /// Crm sistemindeki organization bilgisidir.
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}