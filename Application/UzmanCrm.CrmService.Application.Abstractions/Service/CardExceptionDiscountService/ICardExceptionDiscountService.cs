using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService
{
    public interface ICardExceptionDiscountService : IApplicationService
    {
        Task<Response<CardExceptionDiscountSaveResponseDto>> CardExceptionDiscountSaveAsync(CardExceptionDiscountRequestDto requestDto);
        Task<Response<CardExceptionDiscountDto>> GetCardExceptionDiscountDetailByErpIdAsync(string loyaltyCardId);
        Task<Response<CardExceptionDiscountAndContactDto>> GetCardDiscount(Guid? cardExceptionDiscountId);
        Task<Response<CardExceptionDiscountSaveResponseDto>> UpdateCardExceptionDiscountApprovalStatusAndExplanationAsync(CardApprovalStatusAndExplanationRequestDto requestDto);
        Task<Response<CardExceptionDiscountSaveResponseDto>> UpdateCardExceptionDiscountEndDateAndDiscountRateAsync(CardEndDateRequestDto requestDto);
        Task<Response<List<CardExceptionDiscountAndContactDto>>> GetCardExceptionDiscountsCreatedFromStore();
        Task<Response<List<GetCardExceptionDiscountsWillBeExpiredSoon_ResponseDto>>> GetCardExceptionDiscountsWillBeExpiredSoon(Guid cardClassSegmentId, int remainingPeriodCount);
        Task<Response<List<CardExceptionDiscountSaveResponseDto>>> GetExpiredTodayCardExceptionDiscounts();
        Task<Response<CardExceptionDiscountDto>> CheckIfCardExceptionDiscountExistByErpId(string erpId);
        Task<Response<CardExceptionDiscountDto>> GetCardExceptionDiscountStatusAndApprovalStatusById(Guid? cardExceptionDiscountId);
        Task<Response<List<CardExceptionDiscountDto>>> GetCardExceptionDiscountsByBatchApprovalListId(Guid? batchApprovalListId);
    }
}
