using System.Collections.Generic;

namespace UzmanCrm.CrmService.WebAPI.Models.Campaign
{
    public class CompleteCampaignProcessRequest
    {
        /// <summary>
        /// Kampanya kazanımlarını temsil eden Id bilgisidir. Bu bilgi ile işlem tamamlanabilir.
        /// </summary>
        public string TrackingId { get; set; } = null;
        public List<PaymentModel> Payments { get; set; } = null;

    }

    public class PaymentModel
    {
        /// <summary>
        /// Var ise ödeme alınma işlem ID değeri
        /// </summary>
        public string TransactionId { get; set; } = null;

        /// <summary>
        /// Ödeme yöntemi Enum değer olarak girilmelidir. 
        /// </summary>
        public string PaymentsType { get; set; } = null;

        /// <summary>
        /// Ödemenin alındığı fiyat bilgisi.
        /// </summary>
        public decimal? Total { get; set; } = null;
    }
}