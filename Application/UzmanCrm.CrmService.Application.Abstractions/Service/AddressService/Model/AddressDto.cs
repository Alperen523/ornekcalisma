using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Models
{
    public class AddressDto //uzm_customeraddress
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
        public int? statecode { get; set; } = null;
        public int? statuscode { get; set; } = null;
        public int? uzm_addressecomid { get; set; } = null;
        public string uzm_addressmail { get; set; } = null;
        public string uzm_addressphone { get; set; } = null;
        public int? uzm_addresstype { get; set; } = null;
        public Guid? uzm_cityid { get; set; } = null;
        public string uzm_cityidname { get; set; } = null;
        public Guid? uzm_countryid { get; set; } = null;
        public string uzm_countryidname { get; set; } = null;
        public Guid? uzm_createdbypersonid { get; set; } = null;
        public string uzm_createdbypersonidname { get; set; } = null;
        public Guid? uzm_createdlocationid { get; set; } = null;
        public string uzm_createdlocationidname { get; set; } = null;
        public Guid? uzm_customeraddressid { get; set; } = null;
        public string uzm_customeraddressname { get; set; } = null;
        public Guid? uzm_customerid { get; set; } = null;
        public Guid? uzm_customeridbeforemerge { get; set; } = null;
        public string uzm_customeridbeforemergename { get; set; } = null;
        public string uzm_customeridname { get; set; } = null;
        public int? uzm_datasourceid { get; set; } = null;
        public Guid? uzm_districtid { get; set; } = null;
        public string uzm_districtidname { get; set; } = null;
        public int? uzm_ecomaddressid { get; set; } = null;
        public string uzm_addressecomidstr { get; set; } = null; // (07/2023) int yeterli olmaması üzerine metin olarak oluşturuldu crm tarafında
        public string uzm_fulladdress { get; set; } = null;
        public bool? uzm_isdefaultaddress { get; set; } = null;
        public DateTime? uzm_mergeddate { get; set; } = null;
        public Guid? uzm_modifiedbylocationid { get; set; } = null;
        public string uzm_modifiedbylocationidname { get; set; } = null;
        public Guid? uzm_modifiedbypersonid { get; set; } = null;
        public string uzm_modifiedbypersonidname { get; set; } = null;
        public Guid? uzm_neighborhoodid { get; set; } = null;
        public string uzm_neighborhoodidname { get; set; } = null;
        public string uzm_postcode { get; set; } = null;
        public Guid? uzm_regardingpermissionid { get; set; } = null;
        public string uzm_regardingpermissionidname { get; set; } = null;
        public bool? uzm_statuschangedduetocustomer { get; set; } = null;

        //Custom Field
        public string uzm_ErpId { get; set; } = null;

    }

}
