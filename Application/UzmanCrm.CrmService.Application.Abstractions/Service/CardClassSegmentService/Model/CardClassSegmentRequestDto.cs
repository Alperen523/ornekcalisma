using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService.Model
{
    public class CardClassSegmentRequestDto
    {
        public string SegmentName { get; set; } = null;

        public string Description { get; set; } = null;

        public int? ValidityPeriod { get; set; } = null;

        public int? FirstNotificationPeriod { get; set; } = null;

        public int? SecondNotificationPeriod { get; set; } = null;

        public StatusType StatusEnum { get; set; } = StatusType.Aktif;
    }
}
