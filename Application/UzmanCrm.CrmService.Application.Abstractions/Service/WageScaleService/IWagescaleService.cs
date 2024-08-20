using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.WageScaleService.Model;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService
{
    public interface IWageScaleService : IApplicationService
    {
        Task<Response<List<ValidWageScaleListDto>>> ValidWageScaleGetList();

        Task<Response<CustomerWageScaleResponseDto>> CreateCustomerWagescaleAsync(CreateCustomerWageScaleRequestDto requestDto);

        Task<Response<ValidWageScaleListDto>> GetWagescaleByEndorsement(List<ValidWageScaleListDto> wagescaleList, double validEndorsement, Guid cardTypeId);

        Task<Response<ValidWageScaleListDto>> GetValidWagescale(List<ValidWageScaleListDto> WagescaleList, double ValidEndorsement, CardTypeEnum CardType, Guid cardTypeDefinitionId, CardTypeEnum cardTypeEnum);

        Task<Response<ValidWageScaleListDto>> GetMinWagescaleByCardType(List<ValidWageScaleListDto> wagescaleList, Guid cardTypeId);

        Response<ValidWageScaleListDto> GetBiggerThanZeroDiscountMinWagescaleByCardType(List<ValidWageScaleListDto> wagescaleList, Guid cardTypeId);

        Task<Response<ValidWageScaleListDto>> GetBiggerThanValidEndorsementMinWagescaleByCardType(List<ValidWageScaleListDto> wagescaleList, double validEndorsement, Guid cardTypeId);

        Task<Response<ValidWageScaleListDto>> GetValidUpperWagescale(List<ValidWageScaleListDto> WagescaleList, double ValidEndorsement, CardTypeEnum CardType, Guid cardTypeDefinitionId, CardTypeEnum cardTypeEnum);
    }
}
