using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PortalService.Model
{
    public class PortalUserResponseDto
    {
        //public int? statecode { get; set; }
        //public int? statuscode { get; set; }
        //public Guid? uzm_approvingsupervisorid { get; set; }
        //public string uzm_approvingsupervisoridname { get; set; }
        //public string uzm_eposta { get; set; }
        //public string uzm_firstname { get; set; }
        //public string uzm_fullname { get; set; }
        //public string uzm_jobtitle { get; set; }
        //public string uzm_lastname { get; set; }
        //public string uzm_mobilephone { get; set; }
        //public Guid? uzm_portaluserid { get; set; }
        //public string uzm_username { get; set; }

        public int? statecode { get; set; }
        public string statecodename { get; set; }
        public int? statuscode { get; set; }
        public string statuscodename { get; set; }
        public string uzm_eposta { get; set; }
        public string uzm_firstname { get; set; }
        public string uzm_fullname { get; set; }
        public string uzm_lastname { get; set; }
        public string uzm_mobilephone { get; set; }
        public Guid? uzm_portaluserid { get; set; }
        public string uzm_storecode { get; set; }
        public Guid? uzm_storeid { get; set; }
        public string uzm_storeidname { get; set; }
        public Guid? uzm_organizationid { get; set; }
        public string uzm_organizationidname { get; set; }
        public Guid? uzm_suborganization { get; set; }
        public string uzm_suborganizationname { get; set; }
        public string uzm_username { get; set; }

        //BUSINESS UNIT
        public Guid? uzm_cardexceptiondiscountapprover { get; set; }
        public string uzm_cardexceptiondiscountapproverName { get; set; }
        public Guid? BusinessUnitId { get; set; }
    }
}
