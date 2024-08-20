using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model
{
    public class SearchErpIdRequest
    {
        public ChannelEnum ChannelId { get; set; }
        public string ErpId { get; set; } = null;

    }
}
