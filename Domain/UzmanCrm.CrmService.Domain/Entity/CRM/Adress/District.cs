using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Adress
{
    public class District //uzm_district
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
        public Guid? uzm_district_cityid { get; set; } = null;
        public string uzm_district_cityidname { get; set; } = null;
        public string uzm_districtcode { get; set; } = null;
        public Guid? uzm_districtid { get; set; } = null;
        public string uzm_districtname { get; set; } = null;
        public string uzm_districtno { get; set; } = null;
        public string uzm_ilce_sub_code { get; set; } = null;
        public string uzm_old_district_code { get; set; } = null;
    }
}
