using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService.Model
{
    public class CustomerEndorsementDto
    {
        public Guid? createdby { get; set; } = null;
        public string createdbyname { get; set; } = null;
        public DateTime? createdon { get; set; } = null;
        public int? importsequencenumber { get; set; } = null;
        public Guid? modifiedby { get; set; } = null;
        public string modifiedbyname { get; set; } = null;
        public DateTime? modifiedon { get; set; } = null;
        public Guid? organizationid { get; set; } = null;
        public string organizationidname { get; set; } = null;
        public int? statecode { get; set; } = null;
        public int? statuscode { get; set; } = null;
        public int? uzm_billtype { get; set; } = null;
        public string uzm_cardno { get; set; } = null;
        public Guid? uzm_customerendorsementid { get; set; } = null;
        public string uzm_erpid { get; set; } = null;
        public double? uzm_giftcardamount { get; set; } = 0;
        public bool? uzm_integrationstatus { get; set; } = null;
        public DateTime? uzm_invoicedate { get; set; } = null;
        public string uzm_invoicenumber { get; set; } = null;
        public string uzm_name { get; set; } = null;
        public DateTime? uzm_orderdate { get; set; } = null;
        public string uzm_ordernumber { get; set; } = null;
        public string uzm_storecode { get; set; } = null;
        public double? uzm_totalamount { get; set; } = 0;
        public string uzm_transactionid { get; set; } = null;


    }
}
