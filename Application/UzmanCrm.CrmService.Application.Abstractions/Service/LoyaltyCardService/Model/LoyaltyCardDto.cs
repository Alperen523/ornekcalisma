using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model
{
    public class LoyaltyCardDto
    {
        public DateTime? createdon { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public double? uzm_amountforuppersegmentvakko { get; set; } = null;
        public double? uzm_amountforuppersegmentvr { get; set; } = null;
        public double? uzm_amountforuppersegmentwcol { get; set; } = null;

        public string uzm_cardnumber { get; set; } = null;

        public Guid? uzm_cardtypedefinitionid { get; set; } = null;

        public string uzm_cardtypedefinitionidname { get; set; } = null;

        public Guid? uzm_contactid { get; set; } = null;

        public string uzm_contactidname { get; set; } = null;

        public double? uzm_differenceendorsement { get; set; } = null;

        public string uzm_email { get; set; } = null;

        public string uzm_erpid { get; set; } = null;

        public Guid? uzm_loyaltycardid { get; set; } = null;

        public string uzm_mobilephone { get; set; } = null;

        public double? uzm_periodendorsement { get; set; } = null;

        public int? uzm_statuscode { get; set; } = null;

        public double? uzm_turnoverendorsement { get; set; } = null;

        public double? uzm_uppersegmentdiscountpercentvakko { get; set; } = null;
        public double? uzm_uppersegmentdiscountpercentvr { get; set; } = null;
        public double? uzm_uppersegmentdiscountpercentwcol { get; set; } = null;

        public double? uzm_validdiscountratevakko { get; set; } = 0;
        public double? uzm_validdiscountratevr { get; set; } = 0;
        public double? uzm_validdiscountratewcol { get; set; } = 0;

        public double? uzm_validendorsement { get; set; } = null;

        public string uzm_storecode { get; set; } = null;

        public int? uzm_endorsementtype { get; set; } = null;

        public string modifiedbyname { get; set; } = null;

        public string uzm_carddiscountidname { get; set; } = null;

        public string createdbyname { get; set; } = null;

        public string organizationidname { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? organizationid { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public Guid? uzm_carddiscountid { get; set; } = null;

        public string uzm_subcardnumber { get; set; } = null;

        // Ek alanlar
        public string State { get; set; } = null;
        public string storename { get; set; } = null;
        public Guid? uzm_storeid { get; set; } = null;
    }
}
