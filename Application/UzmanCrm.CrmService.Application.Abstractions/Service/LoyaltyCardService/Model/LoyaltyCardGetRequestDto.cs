using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model
{
    public class LoyaltyCardGetRequestDto
    {
        public string CardNo { get; set; } = null;

        public string ErpId { get; set; } = null;

        public CardTypeEnum ChannelType { get; set; }
    }
}