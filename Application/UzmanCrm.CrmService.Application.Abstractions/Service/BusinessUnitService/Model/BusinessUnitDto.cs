using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService.Model
{
    public class BusinessUnitDto
    {
        public Guid? businessunitid { get; set; } = null;
        public Guid? calendarid { get; set; } = null;
        public string costcenter { get; set; } = null;
        public Guid? createdby { get; set; } = null;
        public string createdbyname { get; set; } = null;
        public DateTime? createdon { get; set; } = null;
        public double? creditlimit { get; set; } = null;
        public string description { get; set; } = null;
        public string disabledreason { get; set; } = null;
        public string divisionname { get; set; } = null;
        public string emailaddress { get; set; } = null;
        public decimal? exchangerate { get; set; } = null;
        public string fileasname { get; set; } = null;
        public string ftpsiteurl { get; set; } = null;
        public int? importsequencenumber { get; set; } = null;
        public int? inheritancemask { get; set; } = null;
        public bool? isdisabled { get; set; } = null;
        public Guid? modifiedby { get; set; } = null;
        public string modifiedbyname { get; set; } = null;
        public DateTime? modifiedon { get; set; } = null;
        public string name { get; set; } = null;
        public Guid? organizationid { get; set; } = null;
        public string organizationidname { get; set; } = null;
        public Guid? parentbusinessunitid { get; set; } = null;
        public string parentbusinessunitidname { get; set; } = null;
        public string picture { get; set; } = null;
        public string stockexchange { get; set; } = null;
        public string tickersymbol { get; set; } = null;
        public Guid? transactioncurrencyid { get; set; } = null;
        public string transactioncurrencyidname { get; set; } = null;
        public Guid? usergroupid { get; set; } = null;
        public int? utcoffset { get; set; } = null;
        public string uzm_accountcode { get; set; } = null;
        public string uzm_address { get; set; } = null;
        public Guid? uzm_bucityid { get; set; } = null;
        public string uzm_bucityidname { get; set; } = null;
        public Guid? uzm_bucountryid { get; set; } = null;
        public string uzm_bucountryidname { get; set; } = null;
        public Guid? uzm_cardexceptiondiscountapprover { get; set; } = null;
        public string uzm_cardexceptiondiscountapprovername { get; set; } = null;
        public DateTime? uzm_closingdate { get; set; } = null;
        public bool? uzm_entegrasyondepartmanm { get; set; } = null;
        public string uzm_manageremail { get; set; } = null;
        public DateTime? uzm_openingdate { get; set; } = null;
        public string uzm_phone { get; set; } = null;
        public DateTime? uzm_sourcemodefiedon { get; set; } = null;
        public int? uzm_storespecies { get; set; } = null;
        public int? uzm_storetype { get; set; } = null;
        public string uzm_tanicode { get; set; } = null;
        public bool? uzm_vipgorunum { get; set; } = null;
        public string websiteurl { get; set; } = null;
        public bool? workflowsuspended { get; set; } = null;
    }

}
