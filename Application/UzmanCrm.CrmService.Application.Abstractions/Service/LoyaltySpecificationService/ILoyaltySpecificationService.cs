using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltySpecificationService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltySpecificationService
{
    public interface ILoyaltySpecificationService : IApplicationService
    {
        Task<Response<ValidLoyaltySpecificationItemDto>> ValidLoyaltySpecificationGetItem();
    }
}
