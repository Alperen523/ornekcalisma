using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.WageScaleService.Model
{
    public class WageScaleDto
    {
        public Guid? uzm_wagescaleid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public double? uzm_rangestart { get; set; } = null;

        public double? uzm_rangeend { get; set; } = null;

        public double? uzm_discountrate { get; set; } = null;

        public DateTime? uzm_wagescaleyear { get; set; } = null;
        
        
        public Guid? uzm_cardtypedefinitionid { get; set; } = null;
    }
}
