using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model
{
    public class CardExceptionDiscountDto
    {
        public Guid? uzm_carddiscountId { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public Guid? createdby { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public Guid? modifiedby { get; set; } = null;

        public Guid? organizationid { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public int? importsequencenumber { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public Guid? uzm_loyaltycardid { get; set; } = null;

        public Guid? uzm_cardclasssegmentid { get; set; } = null;

        public Guid? uzm_customergroupid { get; set; } = null;

        public DateTime? uzm_startdate { get; set; } = null;

        public DateTime? uzm_enddate { get; set; } = null;

        public double? uzm_discountrate { get; set; } = null;

        public Guid? uzm_demandeduser { get; set; } = null;

        public int? uzm_approvalstatus { get; set; } = null;

        public string uzm_description { get; set; } = null;

        public DateTime? uzm_demanddate { get; set; } = null;

        public Guid? uzm_approvedby { get; set; } = null;

        public int? uzm_statuscode { get; set; } = null;

        public int uzm_arrivalstatus { get; set; }

        public string uzm_approvalexplanation { get; set; }

        public string createdbyname { get; set; } = null;

        public string modifiedbyname { get; set; } = null;

        public string organizationidname { get; set; } = null;

        public string uzm_demandedusername { get; set; } = null;

        public string uzm_customergroupidname { get; set; } = null;

        public string uzm_approvedbyname { get; set; } = null;

        public string uzm_loyaltycardidname { get; set; } = null;

        public string uzm_cardclasssegmentidname { get; set; } = null;

        public int? uzm_arrivalchannel { get; set; } = null;

        public Guid? uzm_demandstore { get; set; } = null;

        public Guid? vkk_batchapprovallistid { get; set; } = null;

        public string vkk_erpid { get; set; } = null;

    }
}
