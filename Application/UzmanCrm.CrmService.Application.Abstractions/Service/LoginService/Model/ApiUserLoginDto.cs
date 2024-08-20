using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Login
{
    public class ApiUserLoginDto
    {

        public string modifiedbyname { get; set; } = null;

        public string modifiedbyyominame { get; set; } = null;

        public string createdbyname { get; set; } = null;

        public string createdbyyominame { get; set; } = null;

        public string createdonbehalfbyname { get; set; } = null;

        public string createdonbehalfbyyominame { get; set; } = null;

        public string modifiedonbehalfbyname { get; set; } = null;

        public string modifiedonbehalfbyyominame { get; set; } = null;

        public Guid? ownerid { get; set; } = null;

        public string owneridname { get; set; } = null;

        public string owneridyominame { get; set; } = null;

        public int? owneriddsc { get; set; } = null;

        public int? owneridtype { get; set; } = null;

        public Guid? owninguser { get; set; } = null;

        public Guid? owningteam { get; set; } = null;

        public Guid? uzm_apiuserloginid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? createdonbehalfby { get; set; } = null;

        public Guid? modifiedonbehalfby { get; set; } = null;

        public Guid? owningbusinessunit { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public byte[] versionnumber { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public DateTime? overriddencreatedon { get; set; } = null;

        public int? timezoneruleversionnumber { get; set; } = null;

        public int? utcconversiontimezonecode { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public string uzm_username { get; set; } = null;

        public string uzm_password { get; set; } = null;
        public string uzm_roles { get; set; } = null;


    }
}
