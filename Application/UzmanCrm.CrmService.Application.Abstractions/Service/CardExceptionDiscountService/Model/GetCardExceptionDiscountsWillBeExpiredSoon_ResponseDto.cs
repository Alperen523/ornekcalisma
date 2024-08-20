using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model
{
    public class GetCardExceptionDiscountsWillBeExpiredSoon_ResponseDto
    {
        public string uzm_contactidname { get; set; } = null;

        public string uzm_erpid { get; set; } = null;

        public string uzm_loyaltycardidname { get; set; } = null;

        public string uzm_demandedusername { get; set; } = null;

        public DateTime? uzm_demanddate { get; set; } = null;

        public string uzm_cardclasssegmentidname { get; set; } = null;

        public string uzm_customergroupidname { get; set; } = null;

        public string uzm_arrivalchannelname { get; set; } = null;

        public double? uzm_discountrate { get; set; } = null;

        public DateTime? uzm_startdate { get; set; } = null;

        public DateTime? uzm_enddate { get; set; } = null;

        public string uzm_description { get; set; } = null;

        public string uzm_approvedbyname { get; set; } = null;

        public string uzm_approvalexplanation { get; set; }

        public string uzm_demandstorename { get; set; }
    }
}
