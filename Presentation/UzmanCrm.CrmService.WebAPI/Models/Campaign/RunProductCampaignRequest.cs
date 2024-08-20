using System;
using System.Collections.Generic;

namespace UzmanCrm.CrmService.WebAPI.Models.Campaign
{


    public class RunProductCampaignRequest
    {
        /// <summary>
        /// Crm Contact tablosu unique Id bilgisidir.
        /// </summary>
        public Guid ContactId { get; set; }

        /// <summary>
        /// Fatura numarası unique
        /// </summary>
        public string InvoiceNo { get; set; } = null;

        /// <summary>
        /// Ürünlerin sipariş Id bilgisidir.
        /// </summary>
        public string OrderId { get; set; } = null;

        /// <summary>
        /// Mağaza Kodu veya Id bilgisi. Örneğin: 105 = ANTALYA TERRACİTY MACRO
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// İşlemi yapan kasa ,personel veya uygulama Id bilgisi.  Örneğin: 001
        /// </summary>
        public string PersonId { get; set; }


        /// <summary>
        /// Qr kanalından geliyor ise QR Id bilgisi.
        /// </summary>
        public Guid QrCodeId { get; set; }

        /// <summary>
        /// Fatura tarihi bilgisi.
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Sipariş tarihi bilgisi.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Toplam Fatura veya Sipariş fiyat bilgisi.
        /// </summary>
        public decimal TotalPrice { get; set; } = 0;

        /// <summary>
        /// İndirim tutarı bilgisi.
        /// </summary>
        public decimal DiscountAmount { get; set; } = 0;


        public List<PItem> ProductItems { get; set; }

    }


    public class PItem
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
        /// Ürün Birim fiyat bilgisi.
        /// </summary>
        public decimal UnitePrice { get; set; }

        /// <summary>
        /// Ürünün gramaj/ağırlık bilgisidir.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Ürünün indirim tutarı bilgisi
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Ürünün indirim yüzdesi bilgisi
        /// </summary>
        public decimal DiscountPercentage { get; set; }


        /// <summary>
        /// Ürünün toplam fiyat  bilgisi
        /// </summary>
        public decimal TotalPrice { get; set; }

    }

}
