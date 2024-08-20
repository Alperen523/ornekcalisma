using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService.Model
{
    public class CustomerWageScaleDto
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
        public bool? uzm_activitystatus { get; set; } = null;
        public Guid? uzm_customerendorsementid { get; set; } = null;
        public string uzm_customerendorsementidname { get; set; } = null;
        public Guid? uzm_customerwagescaleid { get; set; } = null;
        public Guid? uzm_loyaltycardid { get; set; } = null;
        public string uzm_loyaltycardidname { get; set; } = null;
        public string uzm_name { get; set; } = null;
        public Guid? uzm_wagescaleidvakko { get; set; } = null;
        public string uzm_wagescaleidvakkoname { get; set; } = null;
        public Guid? uzm_wagescaleidvr { get; set; } = null;
        public string uzm_wagescaleidvrname { get; set; } = null;
        public Guid? uzm_wagescaleidwcol { get; set; } = null;
        public string uzm_wagescaleidwcolname { get; set; } = null;

        public double? uzm_carddiscount_discountrate { get; set; } = null;

        public double? uzm_validdiscountratevakko { get; set; } = null;

        public double? uzm_validdiscountratevr { get; set; } = null;

        public double? uzm_validdiscountratewcol { get; set; } = null;

        public double? uzm_periodendorsement { get; set; } = null;

        public double? uzm_turnoverendorsement { get; set; } = null;

    }
}
