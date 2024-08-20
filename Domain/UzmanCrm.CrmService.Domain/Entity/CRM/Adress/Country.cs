using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Adress
{
    public class Country //uzm_countries
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
        public Guid? uzm_countriesid { get; set; } = null;
        public string uzm_countrycode { get; set; } = null;
        public string uzm_countryname { get; set; } = null;
        public string uzm_isocode { get; set; } = null;
    }
}
