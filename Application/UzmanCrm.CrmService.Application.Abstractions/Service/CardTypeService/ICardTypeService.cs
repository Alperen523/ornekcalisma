using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService
{
    public interface ICardTypeService : IApplicationService
    {
        Task<Response<CardTypeDto>> GetCardTypeByNameItemAsync(CardTypeEnum cardTypeName);
        Task<Response<CardTypeDto>> GetCardTypeByCodeItemAsync(string cardCode);
        Task<Response<CardTypeDto>> GetCardTypeByIdItemAsync(string cardTypeId);
    }
}
