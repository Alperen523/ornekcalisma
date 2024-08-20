using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService
{

    public interface IPhoneService : IApplicationService
    {
        Task<Response<PhoneSaveResponse>> PhoneSaveAsync(PhoneSaveRequestDto requestDto);

        Task<Response<int>> DeleteContactPhoneControl(DeletePhoneRequestDto requestDto);
    }
}
