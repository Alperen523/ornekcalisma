using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService.Model
{
    public class EndorsementRequestDto
    {

        public string ErpId { get; set; } = null;


        public string InvoiceNumber { get; set; } = null;


        public DateTime? OrderDate { get; set; } = null;


        public string OrderNumber { get; set; } = null;


        public string StoreCode { get; set; }


        public decimal? TotalAmount { get; set; } = null;

        public string TransactionId { get; set; } = null;


        public decimal? GiftCardAmount { get; set; } = null;


        public BillTypeEnum BillType { get; set; } = BillTypeEnum.Unknown;
        public string CardNo { get; set; }

        public DateTime? InvoiceDate { get; set; } = null;

    }

}
