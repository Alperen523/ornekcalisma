using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Contact
{
    public class Old_CustomerPermission
    {
        public string uzm_customeridName { get; set; } = null;

        public string uzm_createdpersonidName { get; set; } = null;

        public string CreatedByName { get; set; } = null;

        public string uzm_createdlocationidName { get; set; } = null;

        public string ModifiedByName { get; set; } = null;

        public string uzm_modifiedbypersonidName { get; set; } = null;

        public string OrganizationIdName { get; set; } = null;

        public string uzm_modifiedbylocationidName { get; set; } = null;

        public Guid? uzm_customerpermissionsId { get; set; } = null;

        public DateTime? CreatedOn { get; set; } = null;

        public Guid? CreatedBy { get; set; } = null;

        public DateTime? ModifiedOn { get; set; } = null;

        public Guid? ModifiedBy { get; set; } = null;

        public Guid? OrganizationId { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public int? ImportSequenceNumber { get; set; } = null;

        public string uzm_documentno { get; set; } = null;

        public Guid? processid { get; set; } = null;

        public Guid? stageid { get; set; } = null;

        public string traversedpath { get; set; } = null;

        public Guid? uzm_customerid { get; set; } = null;

        public Guid? uzm_createdlocationid { get; set; } = null;

        public Guid? uzm_createdpersonid { get; set; } = null;

        public Guid? uzm_modifiedbylocationid { get; set; } = null;

        public Guid? uzm_modifiedbypersonid { get; set; } = null;

        public int? uzm_versionno { get; set; } = null;

        public int? uzm_type { get; set; } = null;

        public int? uzm_statuscode { get; set; } = null;

        public DateTime? uzm_approvaldate { get; set; } = null;

        public string uzm_approvalnumber { get; set; } = null;
    }
}
