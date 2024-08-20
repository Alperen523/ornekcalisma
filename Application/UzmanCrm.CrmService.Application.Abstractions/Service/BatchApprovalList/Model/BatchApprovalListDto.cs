using System;

namespace Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList.Model
{
    public class BatchApprovalListDto
    {
        public string createdbyname { get; set; } = null;

        public string vkk_approvedbyname { get; set; } = null;

        public string vkk_demandedusername { get; set; } = null;

        public string organizationidname { get; set; } = null;

        public string vkk_demandstorename { get; set; } = null;

        public string modifiedbyname { get; set; } = null;

        public Guid? vkk_batchapprovallistid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? organizationid { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public string vkk_name { get; set; } = null;

        public Guid? vkk_demandeduser { get; set; } = null;

        public Guid? vkk_demandstore { get; set; } = null;

        public DateTime? vkk_demanddate { get; set; } = null;

        public int? vkk_approvalstatus { get; set; } = null;

        public int? vkk_arrivalchannel { get; set; } = null;

        public Guid? vkk_approvedby { get; set; } = null;

        public string vkk_approvalexplanation { get; set; } = null;

        public string vkk_batchapprovallistcode { get; set; } = null;

        public int? vkk_processstatus { get; set; } = null;

    }

}
