using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.Shared
{
    public class BaseRequestDto
    {
        public ChannelEnum ChannelId { get; set; } = ChannelEnum.Bilinmiyor;
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;
        public string StoreCode { get; set; } = null;
        public string PersonNo { get; set; } = null;
        public Guid? StoreId { get; set; } = null;
        public Guid? PersonId { get; set; } = null;
        public OrganizationEnum Organization { get; set; } = OrganizationEnum.TR;
        public CompanyEnum Company { get; set; } = CompanyEnum.KD;
    }
}
