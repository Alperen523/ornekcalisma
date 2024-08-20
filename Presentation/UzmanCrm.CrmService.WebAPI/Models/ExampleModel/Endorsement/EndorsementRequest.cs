using FluentValidation.Attributes;
using System;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.Endorsement
{
    [Validator(typeof(EndorsementRequestValidator))]
    /// <summary>
    /// Ciro istek modeli
    /// </summary>
    public class EndorsementRequest
    {
        /// <summary>
        /// Erp sistemindeki benzersiz müşteri Id bilgisidir.
        /// </summary>
        public string ErpId { get; set; } = null;

        /// <summary>
        /// Fatura oluşturma tarihi bilgisidir.
        /// </summary>
        public DateTime? InvoiceDate { get; set; } = null;

        /// <summary>
        /// Fatura numarası bilgisidir.
        /// </summary>
        public string InvoiceNumber { get; set; } = null;

        /// <summary>
        /// Fatura üzerindeki sipariş tarihi bilgisidir.
        /// </summary>
        public DateTime? OrderDate { get; set; } = null;

        /// <summary>
        /// Fatura üzerindeki sipariş numarası bilgisidir.
        /// </summary>
        public string OrderNumber { get; set; } = null;

        /// <summary>
        /// Faturanın oluşturulduğu mağaza kodu bilgisidir.
        /// </summary>
        public string StoreCode { get; set; }

        /// <summary>
        /// Fatura üzerindeki tutar (ciro) bilgisidir.
        /// </summary>
        public decimal? TotalAmount { get; set; } = null;
        /// <summary>
        /// SAP tarafından kaydın takip edilebileceği Id bilgisidir.SAP tarafındaki satırın benzersiz id bilgisi olabilir.
        /// </summary>
        public string TransactionId { get; set; } = null;

        /// <summary>
        /// Fatura üzerinde kullanılan Gift Kart tutarı bilgisidir.
        /// </summary>
        public decimal? GiftCardAmount { get; set; } = null;


        /// <summary>
        /// Fatura işlem tipi bilgisidir. (Satış veya İade Faturası gibi)
        /// <br/> ZP01=1  P.Satış - Normal Fatura
        /// <br/> ZP02=2  P.Satış - KDVsiz Fatura
        /// <br/> ZP03=3  P.Satış - Taxfree Fatura
        /// <br/> ZP04=4  P.Satış - Web Fatura
        /// <br/> ZP05=5  P.Satış - E-fatura
        /// <br/> ZP08=6  P.Satış - Gift Card
        /// <br/> ZP09=7  P.Satış - Toplu Gift Kart
        /// <br/> ZP12=8  P.Satış - RB Değişim
        /// <br/> ZP16=9  P.Satış - Tahsilat
        /// <br/> ZP61=10 P.Satış - Kapora
        /// <br/> ZR01=11 P.İade -  Normal Fatura
        /// <br/> ZR02=12 P.İade -  KDVsiz Fatura
        /// <br/> ZR03=13 P.İade -  Taxfree Fatura
        /// <br/> ZR04=14 P.İade -  Web Fatura
        /// <br/> ZR05=15 P.İade -  E-fatura
        /// <br/> ZR08=16 P.İade -  Gift Card
        /// <br/> ZR09=17 P.İade -  Toplu Gift Kart
        /// <br/> ZR12=18 P.İade -  RB Değişim
        /// <br/> ZR17=19 P.İade -  Tediye
        /// <br/> ZR63=20 P.İade -  Kapora İade
        /// <br/> IsDelete= 30 -  Eşleşen Satış faturasını iptal eder.
        /// <br/> IsDeleteReturn= 40 -  Eşleşen İade faturasını iptal eder.
        /// </summary>
        public BillTypeEnum BillType { get; set; } = BillTypeEnum.Unknown;

        /// <summary>
        /// Müşteri Kart numarası bilgisidir.
        /// </summary>
        public string CardNo { get; set; } = null;

    }
}