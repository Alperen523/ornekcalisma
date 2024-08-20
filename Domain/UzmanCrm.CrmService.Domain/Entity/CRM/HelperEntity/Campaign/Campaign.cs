using System;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Campaign
{
    public class Campaign
    {
        public double? uzm_pointlimit { get; set; } = null;

        public DateTime? uzm_campaignenddate { get; set; } = null;

        public DateTime? uzm_campaignstartdate { get; set; } = null;

        public DateTime? uzm_pointstartdate { get; set; } = null;

        public bool? uzm_ismanuelchange { get; set; } = null;

        public double? uzm_fixedpoint { get; set; } = null;

        public int? uzm_pointcategory { get; set; } = null;

        public string uzm_daylimit { get; set; } = null;

        public double? uzm_limit { get; set; } = null;

        public double? uzm_point { get; set; } = null;

        public string uzm_posmessage { get; set; } = null;

        public int? uzm_typeofspending { get; set; } = null;

        public double? uzm_pointrate { get; set; } = null;

        public DateTime? uzm_pointexpiredate { get; set; } = null;

        public string Name { get; set; } = null;

        public string Description { get; set; } = null;

        public string CodeName { get; set; } = null;

        public Guid? ModifiedBy { get; set; } = null;

        public string Message { get; set; } = null;

        public Guid? CampaignId { get; set; } = null;

        public Guid? CreatedBy { get; set; } = null;

        public DateTime? ModifiedOn { get; set; } = null;

        public string PromotionCodeName { get; set; } = null;

        public DateTime? CreatedOn { get; set; } = null;

        public int? TypeCode { get; set; } = null;

        public string ModifiedByName { get; set; } = null;

        public string CreatedByName { get; set; } = null;
    }
}
