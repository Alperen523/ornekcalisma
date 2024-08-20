using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebUI.Models
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

        /// <summary>
        /// Kart Segment bilgisidir.
        /// </summary>
        public List<CardClassSegmentGetResponse> CardClassSegment { get; set; }
    }
}