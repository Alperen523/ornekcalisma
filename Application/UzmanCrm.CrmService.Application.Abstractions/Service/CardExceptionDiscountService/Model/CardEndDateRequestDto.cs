using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model
{
    public class CardEndDateRequestDto
    {
        public Guid? CardDiscountId { get; set; } = null;
        public DateTime EndDate { get; set; }
        public double? DiscountRate { get; set; } = null;
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;

    }
}
