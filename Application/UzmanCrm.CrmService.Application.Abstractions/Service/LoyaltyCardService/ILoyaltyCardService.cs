using System;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService
{
    public interface ILoyaltyCardService : IApplicationService
    {
        Task<Response<LoyaltyCardDto>> LoyaltyCardByErpIdOrCardNoGetAsync(LoyaltyCardGetRequestDto requestDto);

        Task<Response<LoyaltyCardSaveResponseDto>> LoyaltyCardSaveAsync(LoyaltyCardRequestDto requestDto, Response<CardExceptionDiscountDto> cardExceptionDiscountRes);

        Task<Response<LoyaltyCardDto>> GetLoyaltyCardByCardNoAndCardTypeAsync(string cardNo, string cardTypeId);

        Task<Response<LoyaltyCardUpdateResponseDto>> LoyaltyCardUpdateAsync(LoyaltyCardUpdateDto requestDto);

        Task<Response<LoyaltyCardUpdateResponseDto>> LoyaltyCardEndorsementFieldsUpdateWithSqlAsync(LoyaltyCardUpdateDto requestDto);

        Task<Response<LoyaltyCardUpdateResponseDto>> LoyaltyCardDiscountFieldsUpdateWithSqlAsync(LoyaltyCardUpdateDto requestDto);

        Task<Response<LoyaltyCardDto>> GetLoyaltyCardByErpIdAsync(string erpId);

        Task<Response<LoyaltyCardDto>> GetLoyaltyCardByIdAsync(Guid Id);

        Task<Response<LoyaltyCardDto>> GetLoyaltyCardByCardNoAsync(string cardNo);
        
        Task<Response<LoyaltyCardDto>> PersonelDiscountGetAsync(string contactName, string erpId);
    }
}
