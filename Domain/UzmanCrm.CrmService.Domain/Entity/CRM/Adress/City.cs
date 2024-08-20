using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Adress
{
    public class City //uzm_cities
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
        public Guid? uzm_citiesid { get; set; } = null;
        public Guid? uzm_city_countryid { get; set; } = null;
        public string uzm_city_countryidname { get; set; } = null;
        public string uzm_citycode { get; set; } = null;
        public string uzm_cityname { get; set; } = null;
        public string uzm_oldid { get; set; } = null;
        public string uzm_phonecode { get; set; } = null;
        public string uzm_platecode { get; set; } = null;
        public bool? uzm_takeaway { get; set; } = null;
    }
}
