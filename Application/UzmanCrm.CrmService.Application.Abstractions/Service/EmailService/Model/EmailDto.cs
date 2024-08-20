using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model
{
    public class EmailDto //uzm_customeremail
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
        public Guid? uzm_contactid { get; set; } = null;
        public Guid? uzm_createdbypersonid { get; set; } = null;
        public Guid? uzm_createdbystoreid { get; set; } = null;
        public Guid? uzm_customeremailid { get; set; } = null;
        public string uzm_emailaddress { get; set; } = null;
        public DateTime? uzm_emailiyssenddate { get; set; } = null;
        public bool? uzm_emailiyssendstatus { get; set; } = null;
        public Guid? uzm_emailoptinchannelid { get; set; } = null;
        public DateTime? uzm_emailoptindate { get; set; } = null;
        public Guid? uzm_emailoptoutchannelid { get; set; } = null;
        public DateTime? uzm_emailoptoutdate { get; set; } = null;
        public bool? uzm_emailpermission { get; set; } = null;
        public string uzm_emailtransactionid { get; set; } = null;
        public int? uzm_emailtype { get; set; } = null;
        public Guid? uzm_modifiedbypersonid { get; set; } = null;
        public Guid? uzm_modifiedbystoreid { get; set; } = null;
        public string uzm_name { get; set; } = null;
    }



}
