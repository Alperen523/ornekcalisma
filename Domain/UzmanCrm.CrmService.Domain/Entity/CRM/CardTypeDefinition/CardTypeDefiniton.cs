using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.CardTypeDefinition
{
    // uzm_cardtypedefinition
    public class CardTypeDefiniton
    {
        public string organizationidname { get; set; } = null;

        public string createdbyname { get; set; } = null;

        public string modifiedbyname { get; set; } = null;

        public Guid? uzm_cardtypedefinitionid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? organizationid { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public string uzm_cardtypedescription { get; set; } = null;

        public string uzm_code { get; set; } = null;
    }
}
