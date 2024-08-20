using System;
using System.Collections.Generic;

namespace UzmanCrm.CrmService.WebAPI.Models.Campaign
{




    public class RunProductCampaignResponse
    {
        /// <summary>
        /// Crm Contact tablosu unique Id bilgisidir.
        /// </summary>
        public Guid ContactId { get; set; }

        /// <summary>
        /// Mağaza Kodu veya Id bilgisi. Örneğin: 105 = ANTALYA TERRACİTY MACRO
        /// </summary>
        public string StoreId { get; set; }


        /// <summary>
        /// İşlemi yapan kasa ,personel veya uygulama Id/Code bilgisi.  Örneğin: 10762 (EmployeeCode)
        /// </summary>
        public string PersonId { get; set; }


        /// <summary>
        /// Fatura numarası unique
        /// </summary>
        public string InvoiceNo { get; set; } = null;

        /// <summary>
        /// Ürünlerin sipariş Id bilgisidir.
        /// </summary>
        public string OrderId { get; set; } = null;

        public List<ProductItem> ProductItems { get; set; }

        /// <summary>
        /// Kampanya kazanımlarını temsil eden Id bilgisidir. Bu bilgi ile işlem tamamlanabilir.
        /// </summary>
        public string TrackingId { get; set; } = null;


        public class ProductItem
        {
            /// <summary>
            /// Satır unique transaction Id bilgisi.
            /// </summary>
            public string ProductItemId { get; set; }

            /// <summary>
            /// Ürün barkod bilgisi.
            /// </summary>
            public string Barcode { get; set; }

            /// <summary>
            /// Ürün adet bilgisi.
            /// </summary>
            public int Quantity { get; set; }

            /// <summary>
            /// Ürün kodu bilgisi.
            /// </summary>
            public string ProductCode { get; set; }

            /// <summary>
            /// Ürünün gramaj/ağırlık bilgisidir.
            /// </summary>
            public int Weight { get; set; }


            /// <summary>
            /// Ürünün toplam fiyat  bilgisi
            /// </summary>
            public decimal TotalPrice { get; set; }

            public List<ProductItemUsedCampaign> ProductItemUsedCampaign { get; set; }


        }
        public class ProductItemUsedCampaign
        {
            public Guid CampaignId { get; set; }

            /// <summary>
            /// Kampanya kazanım Id bilgisidir.
            /// </summary>
            public Guid CampaignEarningId { get; set; }
            public string CampaignCode { get; set; }
            public string CampaignName { get; set; }
            public string LongDescription { get; set; }
            public string ShortDescription { get; set; }

            /// <summary>
            /// Hesaplanan puan miktarı
            /// </summary>
            public decimal PointAmount { get; set; }

            public int GiftCount { get; set; }

            /// <summary>
            /// Kasada Görünme Durumu, görünür ise : true, değilse : false
            /// </summary>
            public bool IsVisibleAtCashDesk { get; set; }
        }
    }
}