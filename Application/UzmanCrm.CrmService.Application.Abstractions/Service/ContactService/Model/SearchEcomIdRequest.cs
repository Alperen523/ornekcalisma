using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class SearchEcomIdRequest
    {
        public EcomChannelTypeEnum ChannelId { get; set; } = EcomChannelTypeEnum.V;
        public string EcomId { get; set; } = null;

    }
}
