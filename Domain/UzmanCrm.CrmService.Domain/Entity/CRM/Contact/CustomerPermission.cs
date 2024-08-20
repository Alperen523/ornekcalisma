using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Contact
{
    public class CustomerPermission //uzm_customerpermissions
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
        public Guid? processid { get; set; } = null;
        public Guid? stageid { get; set; } = null;
        public int? statecode { get; set; } = null;
        public int? statuscode { get; set; } = null;
        public string traversedpath { get; set; } = null;
        public DateTime? uzm_approvaldate { get; set; } = null;
        public string uzm_approvalnumber { get; set; } = null;
        public Guid? uzm_createdlocationid { get; set; } = null;
        public string uzm_createdlocationidname { get; set; } = null;
        public Guid? uzm_createdpersonid { get; set; } = null;
        public string uzm_createdpersonidname { get; set; } = null;
        public Guid? uzm_customerid { get; set; } = null;
        public Guid? uzm_customeridbeforemerge { get; set; } = null;
        public string uzm_customeridbeforemergename { get; set; } = null;
        public string uzm_customeridname { get; set; } = null;
        public Guid? uzm_customerpermissionsid { get; set; } = null;
        public string uzm_documentno { get; set; } = null;
        public DateTime? uzm_mergeddate { get; set; } = null;
        public Guid? uzm_modifiedbylocationid { get; set; } = null;
        public string uzm_modifiedbylocationidname { get; set; } = null;
        public Guid? uzm_modifiedbypersonid { get; set; } = null;
        public string uzm_modifiedbypersonidname { get; set; } = null;
        public int? uzm_statuscode { get; set; } = null;
        public int? uzm_type { get; set; } = null;
        public int? uzm_versionno { get; set; } = null;
    }

}
