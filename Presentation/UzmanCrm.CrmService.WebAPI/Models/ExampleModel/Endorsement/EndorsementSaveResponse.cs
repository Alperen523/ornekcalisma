using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebAPI.Models.Endorsement
{
    public class EndorsementSaveResponse
    {
        /// <summary>
        /// Crm sistemine kaydedilen benzersiz ciro id bilgisidir.
        /// </summary>
        public Guid Id { get; set; }
    }
}