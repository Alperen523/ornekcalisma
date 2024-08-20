using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService
{


    public interface IEndorsementService : IApplicationService
    {
        Task<Response<EndorsementSaveResponseDto>> EndorsementSaveAsync(EndorsementRequestDto requestDto);
        Task<Response<LoyaltyCardDto>> EndorsementGetAsync(string ErpId, CardTypeEnum CardType);
        Task<Response<List<CustomerEndorsementDto>>> GetUnintegratedEndorsementList();
        Task<Response<EndorsementSaveResponseDto>> SetEndorsementIntegrationStateWithSqlAsync(int integrationStatus, string integrationDescription, string customerEndorsementId, bool discountFixingFlag);
        Task<Response<UpdateResponse>> SetEndorsementWillBePassiveWithSqlAsync(int willBePassive, Guid customerEndorsementId);
    }
}
