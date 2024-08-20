using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.BusinessUnit
{


    public class Store //uzm_storeBase
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
        public string uzm_address { get; set; } = null;
        public bool? uzm_carpark { get; set; } = null;
        public string uzm_casename { get; set; } = null;
        public int? uzm_cashregistercount { get; set; } = null;
        public bool? uzm_chocolateproduction { get; set; } = null;
        public Guid? uzm_cityid { get; set; } = null;
        public string uzm_cityname { get; set; } = null;
        public string uzm_code { get; set; } = null;
        public Guid? uzm_countryid { get; set; } = null;
        public Guid? uzm_countyid { get; set; } = null;
        public string uzm_countyname { get; set; } = null;
        public int? uzm_distributionchannel { get; set; } = null;
        public Guid? uzm_districtid { get; set; } = null;
        public string uzm_fridayclosingtime { get; set; } = null;
        public string uzm_fridayopeningtime { get; set; } = null;
        public bool? uzm_grabandgostore { get; set; } = null;
        public bool? uzm_icecreamproduction { get; set; } = null;
        public bool? uzm_istouristic { get; set; } = null;
        public bool? uzm_isxlstore { get; set; } = null;
        public string uzm_latitude { get; set; } = null;
        public int? uzm_location { get; set; } = null;
        public string uzm_longitude { get; set; } = null;
        public string uzm_mondayclosingtime { get; set; } = null;
        public string uzm_mondayopeningtime { get; set; } = null;
        public string uzm_name { get; set; } = null;
        public bool? uzm_outdoorseatingarea { get; set; } = null;
        public int? uzm_pentiyoung { get; set; } = null;
        public string uzm_phonenumber { get; set; } = null;
        public Guid? uzm_regionalmanagerid { get; set; } = null;
        public Guid? uzm_regionid { get; set; } = null;
        public string uzm_regionname { get; set; } = null;
        public bool? uzm_remoteorder { get; set; } = null;
        public string uzm_saturdayclosingtime { get; set; } = null;
        public string uzm_saturdayopeningtime { get; set; } = null;
        public string uzm_segment { get; set; } = null;
        public string uzm_sondayclosingtime { get; set; } = null;
        public string uzm_sondayopeningtime { get; set; } = null;
        public string uzm_storeclosingtime { get; set; } = null;
        public string uzm_storecode { get; set; } = null;
        public Guid? uzm_storeconceptid { get; set; } = null;
        public string uzm_storeemailaddress { get; set; } = null;
        public Guid? uzm_storegroupid { get; set; } = null;
        public Guid? uzm_storeid { get; set; } = null;
        public Guid? uzm_storemanagerid { get; set; } = null;
        public DateTime? uzm_storeopeningdate { get; set; } = null;
        public string uzm_storeopeninghours { get; set; } = null;
        public string uzm_thursdayclosingtime { get; set; } = null;
        public string uzm_thursdayopeningtime { get; set; } = null;
        public string uzm_tuesdayclosingtime { get; set; } = null;
        public string uzm_tuesdayopeningtime { get; set; } = null;
        public bool? uzm_visibleonmobileapp { get; set; } = null;
        public string uzm_wednesdayclosingtime { get; set; } = null;
        public string uzm_wednesdayopeningtime { get; set; } = null;
        public bool? uzm_wifi { get; set; } = null;
    }



}
