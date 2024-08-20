using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard
{
    public class LoyaltyCardSaveResponse
    {
        /// <summary>
        /// Crm de oluşan sadakat kart kaydının benzersiz id bilgisidir.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Crm kayıtlı benzersiz müşteri id bilgisidir.
        /// </summary>
        public Guid? CrmId { get; set; } = null;

        /// <summary>
        /// Crm de oluşan sadakat kart kaydının kart numarası bilgisidir.
        /// </summary>
        public string CardNo { get; set; } = null;
    }
}