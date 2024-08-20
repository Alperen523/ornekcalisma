using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class LoyaltyCardProfile : Profile
    {
        public LoyaltyCardProfile()
        {
            this.CreateMap<LoyaltyCardGetRequest, LoyaltyCardGetRequestDto>()
                .ReverseMap();
            this.CreateMap<LoyaltyCardRequest, LoyaltyCardRequestDto>()
               .ReverseMap();

            this.CreateMap<LoyaltyCardDto, LoyaltyCardGetResponse>()
                .ForMember(dest => dest.CardNumber, from => from.MapFrom(j => j.uzm_cardnumber))
                .ForMember(dest => dest.CardStatus, from => from.MapFrom(j => j.uzm_statuscode))
                .ForMember(dest => dest.CardTypeDefinitionName, from => from.MapFrom(j => j.uzm_cardtypedefinitionidname))
                .ForMember(dest => dest.CustomerName, from => from.MapFrom(j => j.uzm_contactidname))
                .ForMember(dest => dest.ErpId, from => from.MapFrom(j => j.uzm_erpid))
                .ForMember(dest => dest.Id, from => from.MapFrom(j => j.uzm_loyaltycardid))
                .ForMember(dest => dest.PeriodEndorsement, from => from.MapFrom(j => j.uzm_periodendorsement))
                .ForMember(dest => dest.State, from => from.MapFrom(j => j.State))
                .ForMember(dest => dest.TurnoverEndorsement, from => from.MapFrom(j => j.uzm_turnoverendorsement))
                .ForMember(dest => dest.UpdateDate, from => from.MapFrom(j => j.modifiedon))
                .ForMember(dest => dest.StoreName, from => from.MapFrom(j => j.storename))
                .ForMember(dest => dest.CreateDate, from => from.MapFrom(j => j.createdon))
                .ReverseMap();
            
            this.CreateMap<Response<LoyaltyCardDto>, Response<LoyaltyCardGetResponse>>().ReverseMap();

            this.CreateMap<LoyaltyCardSaveResponseDto, LoyaltyCardSaveResponse>()
               .ReverseMap();

            this.CreateMap<Response<LoyaltyCardSaveResponseDto>, Response<LoyaltyCardSaveResponse>>()
                .ReverseMap();
        }
    }
}