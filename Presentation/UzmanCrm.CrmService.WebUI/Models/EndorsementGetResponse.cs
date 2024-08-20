using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebUI.Models
{
    public class EndorsementGetResponse
    {
        /// <summary>
        /// Üst segmente geçebilmek için gerekli olan ciro miktarı bilgisidir.
        /// </summary>
        public double AmountForUpperSegment { get; set; }

        /// <summary>
        /// Sadakat kart tipi bilgisidir.
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// Geçerli indirim oranı bilgisidir.
        /// </summary>
        public double DiscountPercent { get; set; }

        /// <summary>
        /// Üst segmente geçince geçerli olacak indirim oranı bilgisidir.
        /// </summary>
        public double UpperSegmentDiscountPercent { get; set; }

        /// <summary>
        /// Müşteri özelinde geçerli ciro bilgisidir. (Dönem ciro veya devir ciro arasında büyük olan geçerli ciro kabul edilir)
        /// </summary>
        public double Endorsement { get; set; }

        /// <summary>
        /// Standart: Cirodan hesaplanan segment
        /// Dost: İstisna kaydı var
        /// </summary>
        public EndorsementType EndorsementType { get; set; }
    }
}