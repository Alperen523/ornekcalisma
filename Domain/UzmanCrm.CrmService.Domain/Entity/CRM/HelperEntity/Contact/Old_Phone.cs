using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Contact
{
    public class Old_Phone
    {

        public Guid? uzm_customerphoneid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? organizationid { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public string uzm_customerphonenumber { get; set; } = null;

        public Guid? uzm_customerid { get; set; } = null;

        public int? uzm_phonetype { get; set; } = null;

        public Guid? uzm_createdlocationid { get; set; } = null;

        public Guid? uzm_modifiedbylocationid { get; set; } = null;

        public Guid? uzm_createdpersonid { get; set; } = null;

        public Guid? uzm_modifiedbypersonid { get; set; } = null;

        public bool? uzm_phonepermission { get; set; } = null;

        public int? uzm_datasourceid { get; set; } = null;

        public Guid? uzm_releatedpermissionid { get; set; } = null;

        public DateTime? uzm_unsubscribedate { get; set; } = null;

        public Guid? uzm_unsubscribe_channelid { get; set; } = null;

        public bool? uzm_iyscallpermit { get; set; } = null;

        public bool? uzm_iysmessagepermit { get; set; } = null;

        public DateTime? uzm_iysmessagepermitdate { get; set; } = null;

        public DateTime? uzm_iyscallpermitdate { get; set; } = null;

        public string uzm_iyscallid { get; set; } = null;

        public string uzm_iysmessageid { get; set; } = null;

        public bool? uzm_isfigensoft { get; set; } = null;

        public string uzm_confirmcode { get; set; } = null;

        public string uzm_magazakodu { get; set; } = null;

        public string uzm_adi { get; set; } = null;

        public DateTime? uzm_izintarihi { get; set; } = null;

        public bool? uzm_iyscallchange { get; set; } = null;

        public bool? uzm_iysmessagechange { get; set; } = null;

        public bool? uzm_callpermitchecked { get; set; } = null;

        public bool? uzm_smspermitchecked { get; set; } = null;

        public string createdpersonno { get; set; } = null;

        public string updatedpersonno { get; set; } = null;

    }
}
