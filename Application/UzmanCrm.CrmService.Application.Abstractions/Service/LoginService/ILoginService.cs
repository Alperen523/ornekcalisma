using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoginService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Domain.Entity.CRM.Login;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoginService
{
    public interface ILoginService : IApplicationService
    {
        Task<Response<TokenResponse>> Authenticate(ApiUserLoginRequestDto model);
    }
}