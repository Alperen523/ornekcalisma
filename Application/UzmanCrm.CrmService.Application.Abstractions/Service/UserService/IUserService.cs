using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService.Model;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.UserService
{
    public interface IUserService : IApplicationService
    {
        Task<Response<EmployeeDto>> GetEmployeeNumberAsync(string personnelNumber, CompanyEnum company);
    }
}
