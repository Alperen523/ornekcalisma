using System;
using System.Collections.Generic;

namespace UzmanCrm.CrmService.WebAPI.Models.Campaign
{
    public class GetCustomerCampaignInfoResponse
    {
        /// <summary>
        /// Ürün sepetine göre kampanya bilgileri.
        /// </summary>
        public List<Campaigns> Campaigns { get; set; }

        /// <summary>
        /// Müşteri bilgileri.
        /// </summary>
        public CustomerInfo CustomerInfo { get; set; } = null;

    }

    public class CustomerInfo
    {
        /// <summary>
        /// Crm Contact tablosu unique Id bilgisidir.
        /// </summary>
        public Guid? ContactId { get; set; } = null;

        /// <summary>
        /// Crm Contact tablosu Doğum tarihi bilgisidir.
        /// </summary>
        public Guid? BirthDate { get; set; } = null;

        /// <summary>
        /// Crm Contact tablosu Cep telefon bilgisidir.
        /// </summary>
        public string PhoneNumber { get; set; } = null;

        /// <summary>
        /// Müşteri Toplam puan bilgisidir.
        /// </summary>
        public string Point { get; set; } = null;

    }

    public class Campaigns
    {
        public Guid CampaignId { get; set; }
        public string CampaignCode { get; set; }
        public string CampaignName { get; set; }
        public int GiftCount { get; set; }


    }
}