using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.CustomerGroup
{
    // uzm_customergroup
    public class CustomerGroup
    {
        public string createdbyname { get; set; } = null;

        public string modifiedbyname { get; set; } = null;

        public string organizationidname { get; set; } = null;

        public Guid? uzm_customergroupid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? organizationid { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public int? uzm_groupcode { get; set; } = null;
    }
}
