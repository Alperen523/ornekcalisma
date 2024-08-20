using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.User
{
    public class Employee //uzm_employeeBase
    {

        public Guid? createdby { get; set; } = null;
        public DateTime? createdon { get; set; } = null;
        public Guid? modifiedby { get; set; } = null;
        public DateTime? modifiedon { get; set; } = null;
        public Guid? ownerid { get; set; } = null;
        public int? owneridtype { get; set; } = null;
        public Guid? owningbusinessunit { get; set; } = null;
        public int? statecode { get; set; } = null;
        public int? statuscode { get; set; } = null;
        public Guid? uzm_departmentid { get; set; } = null;
        public DateTime? uzm_dimissaldate { get; set; } = null;
        public string uzm_email { get; set; } = null;
        public Guid? uzm_employeeid { get; set; } = null;
        public bool? uzm_integrationuser { get; set; } = null;
        public string uzm_lastname { get; set; } = null;
        public string uzm_mobilephone { get; set; } = null;
        public string uzm_name { get; set; } = null;
        public Guid? uzm_organizationid { get; set; } = null;
        public string uzm_registrationnumber { get; set; } = null;
        public Guid? uzm_storeid { get; set; } = null;
        public string uzm_tcidentificationnumber { get; set; } = null;
        public int? uzm_workingarea { get; set; } = null;
        public int? uzm_workingstatus { get; set; } = null;
        public DateTime? uzm_workstartdate { get; set; } = null;


    }
}