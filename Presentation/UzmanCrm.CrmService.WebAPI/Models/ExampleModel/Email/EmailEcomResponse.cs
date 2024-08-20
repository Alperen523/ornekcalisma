using System;
using System.ComponentModel.DataAnnotations;

namespace UzmanCrm.CrmService.WebAPI.Models.Email
{
    public class EmailEcomResponse
    {

        /// <summary>
        /// Email adresi bilgisidir
        /// </summary>
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Email adresi izin bilgisidir. false=izinsiz , true=izinli
        /// </summary>
        public bool EmailPermit { get; set; } = false;

        /// <summary>
        /// Kaydın son değişiklik tarihi bilgisidir.
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; } = null;
    }
}