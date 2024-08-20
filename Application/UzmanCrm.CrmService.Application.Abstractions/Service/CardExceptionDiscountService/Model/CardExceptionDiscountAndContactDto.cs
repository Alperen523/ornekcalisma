using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model
{
    public class CardExceptionDiscountAndContactDto
    {
        public int? statecode { get; set; }
        public string statecodename { get; set; }
        public int? statuscode { get; set; }
        public string statuscodename { get; set; }
        public string uzm_approvalexplanation { get; set; }
        public int? uzm_approvalstatus { get; set; }
        public string uzm_approvalstatusname { get; set; }
        public Guid? uzm_approvedby { get; set; }
        public string uzm_approvedbyname { get; set; }
        public int? uzm_arrivalchannel { get; set; }
        public string uzm_arrivalchannelname { get; set; }
        public Guid? uzm_cardclasssegmentid { get; set; }
        public string uzm_cardclasssegmentidname { get; set; }
        public Guid? uzm_carddiscountid { get; set; }
        public Guid? uzm_customergroupid { get; set; }
        public string uzm_customergroupidname { get; set; }
        public DateTime? uzm_demanddate { get; set; }
        public Guid? uzm_demandeduser { get; set; }
        public string uzm_demandedusername { get; set; }
        public Guid? uzm_demandstore { get; set; }
        public string uzm_demandstorename { get; set; }
        public string uzm_description { get; set; }
        public double? uzm_discountrate { get; set; }
        public DateTime? uzm_enddate { get; set; }
        public Guid? uzm_loyaltycardid { get; set; }
        public string uzm_loyaltycardidname { get; set; }
        public string uzm_name { get; set; }
        public DateTime? uzm_startdate { get; set; }
        public int? uzm_statuscode { get; set; }
        public string uzm_statuscodename { get; set; }
        public string vkk_erpid { get; set; }


        //PORTALUSER
        public Guid? uzm_portaluserid { get; set; }
        public string uzm_storeidname { get; set; }
        public Guid? uzm_storeid { get; set; }
        public Guid? uzm_organizationid { get; set; }
        public string uzm_organizationidName { get; set; }


        ////LOYALTYCARD
        public string uzm_carddiscountidname { get; set; }
        public string uzm_cardnumber { get; set; }
        public Guid? uzm_contactid { get; set; }
        public string uzm_contactidname { get; set; }
        public double? uzm_differenceendorsement { get; set; }
        public string uzm_email { get; set; }
        public int? uzm_endorsementtype { get; set; }
        public string uzm_erpid { get; set; }
        public string uzm_mobilephone { get; set; }
        public double? uzm_turnoverendorsement { get; set; }
        public double? uzm_uppersegmentdiscountpercent { get; set; }
        public double? uzm_validdiscountrate { get; set; }
        public double? uzm_validdiscountratevakko { get; set; }
        public double? uzm_validdiscountratewcol { get; set; }
        public double? uzm_validdiscountratevr { get; set; }
        public double? uzm_validendorsement { get; set; }
        public double? uzm_periodendorsement { get; set; }

    }
}
