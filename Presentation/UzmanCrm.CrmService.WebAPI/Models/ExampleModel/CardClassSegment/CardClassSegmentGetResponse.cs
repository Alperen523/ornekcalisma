using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UzmanCrm.CrmService.WebAPI.Models.CardClassSegment
{
    public class CardClassSegmentGetResponse
    {
        /// <summary>
        /// Crm sisteminde kayıtlı benzersiz kart sınıfı segmenti id bilgisidir.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Kart sınıfı segmenti isim bilgisidir.
        /// </summary>
        public string SegmentName { get; set; }

        /// <summary>
        /// Kart sınıfı segmenti için eklenmişse açıklama bilgisidir.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// İlgili kart sınıfı segmenti için tanımlanacak kart istisna tanımları default geçerlilik tarihi bilgisidir.
        /// </summary>
        public int ValidityPeriod { get; set; }

        /// <summary>
        /// İlgili kart sınıfı segmenti için tanımlanacak kart istisna tanımları ilk bildirim tarihi bilgisidir. İstisna bitimine belirlenen süre kadar süre kala yetkili kişiye bilgilendirme maili gönderilir.
        /// </summary>
        public int FirstNotificationPeriod { get; set; }

        /// <summary>
        /// İlgili kart sınıfı segmenti için tanımlanacak kart istisna tanımları ikinci bildirim tarihi bilgisidir. İstisna bitimine belirlenen süre kadar süre kala yetkili kişiye bilgilendirme maili gönderilir.
        /// </summary>
        public int SecondNotificationPeriod { get; set; }
    }
}