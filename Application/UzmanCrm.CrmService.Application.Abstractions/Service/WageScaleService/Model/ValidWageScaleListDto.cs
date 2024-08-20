using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.WageScaleService.Model
{
    public class ValidWageScaleListDto
    {
        public Guid? uzm_wagescaleid { get; set; } = null;

        public double? uzm_rangestart { get; set; } = null;

        public double? uzm_rangeend { get; set; } = null;

        public double? uzm_discountrate { get; set; } = null;

        public Guid? uzm_cardtypedefinitionid { get; set; } = null;

    }
}