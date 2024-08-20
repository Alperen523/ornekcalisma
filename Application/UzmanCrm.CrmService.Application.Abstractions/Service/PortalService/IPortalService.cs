using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.PortalService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PortalService
{
    public interface IPortalService : IApplicationService
    {
        Task<Response<PortalUserResponseDto>> GetPortalUserAndApprovedBy(PortalUserRequestDto req);
    }
}
