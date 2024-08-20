using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Contact
{
    public class DataSource // uzm_customerdatasource
    {
        public Guid? createdby { get; set; } = null;
        public string createdbyname { get; set; } = null;
        public DateTime? createdon { get; set; } = null;
        public int? importsequencenumber { get; set; } = null;
        public Guid? modifiedby { get; set; } = null;
        public string modifiedbyname { get; set; } = null;
        public DateTime? modifiedon { get; set; } = null;
        public Guid? ownerid { get; set; } = null;
        public int? owneriddsc { get; set; } = null;
        public string owneridname { get; set; } = null;
        public int? owneridtype { get; set; } = null;
        public Guid? owningbusinessunit { get; set; } = null;
        public Guid? owningteam { get; set; } = null;
        public Guid? owninguser { get; set; } = null;
        public int? statecode { get; set; } = null;
        public int? statuscode { get; set; } = null;
        public Guid? uzm_customerdatasourceid { get; set; } = null;
        public string uzm_customerexternalid { get; set; } = null;
        public Guid? uzm_customerid { get; set; } = null;
        public Guid? uzm_customeridbeforemerge { get; set; } = null;
        public string uzm_customeridbeforemergename { get; set; } = null;
        public string uzm_customeridname { get; set; } = null;
        public Guid? uzm_datasourceid { get; set; } = null;
        public string uzm_datasourceidname { get; set; } = null;
        public string uzm_description { get; set; } = null;
        public string uzm_email { get; set; } = null;
        public string uzm_flag { get; set; } = null;
        public DateTime? uzm_mergeddate { get; set; } = null;
        public string uzm_name { get; set; } = null;
        public bool? uzm_personelgeiiaret { get; set; } = null;
        public string uzm_phone { get; set; } = null;
        public bool? uzm_statuschangedduetocustomer { get; set; } = null;
        public bool? uzm_unusedflag { get; set; } = null;
    }


}
