using FluentValidation.Attributes;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.CardClassSegment
{
    [Validator(typeof(CardClassSegmentRequestValidator))]
    /// <summary>
    /// Kart Sınıfı Segmenti istek modeli
    /// </summary>
    public class CardClassSegmentRequest
    {
        /// <summary>
        /// Kart Sınıfı Segmenti adı bilgisidir
        /// </summary>
        public string SegmentName { get; set; } = null;

        /// <summary>
        /// Segment ile ilgili isteğe bağlı açıklama bilgisidir
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// Kart istisna indirimi için kullanılan varsayılan indirim geçerlilik süresini belirtir
        /// </summary>
        public int? ValidityPeriod { get; set; } = null;

        /// <summary>
        /// İstisna bitimine kaç gün kala birinci mail gönderilsin bilgisidir
        /// </summary>
        public int? FirstNotificationPeriod { get; set; } = null;

        /// <summary>
        /// İstisna bitimine kaç gün kala ikinci mail gönderilsin bilgisidir
        /// </summary>
        public int? SecondNotificationPeriod { get; set; } = null;
    }
}