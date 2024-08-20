using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Adress
{
    public class Neighborhood //uzm_neighborhoods
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
        public Guid? uzm_neighborhood_districtid { get; set; } = null;
        public string uzm_neighborhood_districtidname { get; set; } = null;
        public int? uzm_neighborhoodcode { get; set; } = null;
        public string uzm_neighborhoodname { get; set; } = null;
        public string uzm_neighborhoodnumber { get; set; } = null;
        public Guid? uzm_neighborhoodsid { get; set; } = null;
    }
}
