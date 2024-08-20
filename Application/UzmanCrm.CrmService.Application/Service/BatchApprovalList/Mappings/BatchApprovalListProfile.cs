using AutoMapper;
using Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList.Model;

namespace Vakko.CrmService.Application.Service.BatchApprovalList.Mappings
{
    public class BatchApprovalListProfile : Profile
    {
        public BatchApprovalListProfile()
        {
            this.CreateMap<BatchApprovalListProcessStatusRequestDto, BatchApprovalListDto>()
                .ForMember(dest => dest.vkk_batchapprovallistid, from => from.MapFrom(j => j.BatchApprovalListId))
                .ForMember(dest => dest.vkk_processstatus, from => from.MapFrom(j => j.ProcessStatus))
                .ReverseMap();
        }
    }
}
