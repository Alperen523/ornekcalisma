using System;
using System.ComponentModel.DataAnnotations;

namespace UzmanCrm.CrmService.WebAPI.Models.Phone
{
    public class PhoneEcomResponse
    {
        /// <summary>
        /// Ülke kodu olmayan telefon bilgisi. Örn: "5551234567"
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Sms (Mesaj) izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? SmsPermit { get; set; } = false;

        /// <summary>
        /// Arama izin bilgisi false=izinsiz , true=izinli
        /// </summary>
        public bool? CallPermit { get; set; } = false;

        /// <summary>
        /// Kaydın son değişiklik tarihi bilgisidir.
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; } = null;
    }
}