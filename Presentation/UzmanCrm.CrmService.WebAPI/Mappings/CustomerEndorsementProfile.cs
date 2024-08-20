using AutoMapper;
using NLog.Filters;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.WebAPI.Models.Endorsement;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class CustomerEndorsementProfile : Profile
    {
        public CustomerEndorsementProfile()
        {
            this.CreateMap<EndorsementRequest, EndorsementRequestDto>().ReverseMap();

            this.CreateMap<EndorsementSaveResponseDto, EndorsementSaveResponse>().ReverseMap();

            this.CreateMap<Response<EndorsementSaveResponseDto>, Response<EndorsementSaveResponse>>().ReverseMap();

            this.CreateMap<LoyaltyCardDto, EndorsementGetResponse>()
                .ForMember(dest => dest.CardType, from => from.MapFrom(j => j.uzm_cardtypedefinitionidname))
                .ForMember(dest => dest.Endorsement, from => from.MapFrom(j => j.uzm_validendorsement))
                .ForMember(dest => dest.EndorsementType, from => from.MapFrom(j => j.uzm_endorsementtype))
                .ReverseMap();

            this.CreateMap<Response<LoyaltyCardDto>, Response<EndorsementGetResponse>>().ReverseMap();
        }
    }
}