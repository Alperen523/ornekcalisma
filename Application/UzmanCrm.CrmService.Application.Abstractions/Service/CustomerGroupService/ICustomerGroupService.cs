using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService
{
    public interface ICustomerGroupService : IApplicationService
    {
        Task<Response<List<CustomerGroupGetDto>>> GetCustomerGroupListAsync(string CustomerGroupCode);
    }
}
