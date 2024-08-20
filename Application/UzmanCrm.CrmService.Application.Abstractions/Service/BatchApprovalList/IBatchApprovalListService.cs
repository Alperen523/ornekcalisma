using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList.Model;

namespace Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList
{
    public interface IBatchApprovalListService : IApplicationService
    {
        Task<Response<List<BatchApprovalListDto>>> GetWillBeProcessedBatchApprovalList();
        Task<Response<BatchApprovalListResponseDto>> UpdateBatchApprovalListProcessStatusAsync(BatchApprovalListProcessStatusRequestDto requestDto);
    }
}
