using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService
{
    public interface ICardClassSegmentService : IApplicationService
    {
        Task<Response<CardClassSegmentSaveResponseDto>> CardClassSegmentSaveAsync(CardClassSegmentRequestDto requestDto);
        Task<Response<List<CardClassSegmentDto>>> GetCardClassSegmentListAsync(string SegmentName);
    }
}
