using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.CardExceptionDiscount;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class CardExceptionDiscountProfile : Profile
    {
        public CardExceptionDiscountProfile()
        {
            this.CreateMap<CardExceptionDiscountRequest, CardExceptionDiscountRequestDto>()
                .ReverseMap();

            this.CreateMap<CardExceptionDiscountSaveResponseDto, CardExceptionDiscountSaveResponse>()
                .ReverseMap();

            this.CreateMap<Response<CardExceptionDiscountSaveResponseDto>, Response<CardExceptionDiscountSaveResponse>>()
                .ReverseMap();
        }
    }
}