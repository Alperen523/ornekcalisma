using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService.Model
{
    public class CardTypeDto
    {
        public Guid? uzm_cardtypedefinitionid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public int? statecode { get; set; } = null;

        public int? statuscode { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public string uzm_cardtypedescription { get; set; } = null;

        public string uzm_code { get; set; } = null;
    }
}
