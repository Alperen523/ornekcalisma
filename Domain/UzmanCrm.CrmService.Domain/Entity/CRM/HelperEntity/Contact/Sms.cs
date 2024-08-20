using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Contact
{
    public class Sms
    {
        public Guid? uzm_smsid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? organizationid { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public DateTime? uzm_baslangictarihi { get; set; } = null;

        public Guid? uzm_caseid { get; set; } = null;

        public string uzm_ceptelefonu { get; set; } = null;

        public Guid? uzm_contactid { get; set; } = null;

        public string uzm_durumaciklamasi { get; set; } = null;

        public string uzm_hatakodu { get; set; } = null;

        public int? uzm_iletidurumu { get; set; } = null;

        public DateTime? uzm_iletilmetarihi { get; set; } = null;

        public Guid? uzm_kampanya { get; set; } = null;

        public Guid? uzm_kampanyaaktivitesi { get; set; } = null;

        public string uzm_mesaj { get; set; } = null;

        public Guid? uzm_smsgatewayservisi { get; set; } = null;

        public string uzm_smsproviderid { get; set; } = null;

        public Guid? uzm_businessunitid { get; set; } = null;

        public string uzm_smsresultid { get; set; } = null;

        public bool? uzm_isnotifyurl { get; set; } = null;

        public string uzm_statusname { get; set; } = null;

        public string uzm_statusdescription { get; set; } = null;

        public string uzm_errorname { get; set; } = null;

        public string uzm_errordescription { get; set; } = null;

        public string uzm_currency { get; set; } = null;

        public string uzm_pricepermessage { get; set; } = null;

        public int? uzm_channelid { get; set; } = null;

        public string uzm_bulkid { get; set; } = null;

        public string uzm_provideruser { get; set; } = null;

    }
}
