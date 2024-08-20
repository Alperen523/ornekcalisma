using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.WageScaleService.Model;
using UzmanCrm.CrmService.Domain.Entity.CRM.Wagescale;

namespace UzmanCrm.CrmService.Application.Service.WageScaleService.Mappings
{
    public class WageScaleProfile : Profile
    {
        public WageScaleProfile()
        {
            this.CreateMap<CustomerWageScaleDto, CustomerWageScale>().ReverseMap();


            this.CreateMap<CreateCustomerWageScaleRequestDto, CustomerWageScaleDto>()
                .ForMember(dest => dest.uzm_activitystatus, from => from.MapFrom(j => j.ActivityStatus))
                .ForMember(dest => dest.uzm_carddiscount_discountrate , from => from.MapFrom(j => j.CardDiscount_DiscountRate))
                .ForMember(dest => dest.uzm_customerendorsementid, from => from.MapFrom(j => j.EndorsementId))
                .ForMember(dest => dest.uzm_loyaltycardid, from => from.MapFrom(j => j.LoyaltyCardId))
                .ForMember(dest => dest.uzm_periodendorsement, from => from.MapFrom(j => j.PeriodEndorsement))
                .ForMember(dest => dest.uzm_turnoverendorsement, from => from.MapFrom(j => j.TurnoverEndorsement))
                .ForMember(dest => dest.uzm_validdiscountratevakko , from => from.MapFrom(j => j.ValidDiscountRateVakko))
                .ForMember(dest => dest.uzm_validdiscountratevr , from => from.MapFrom(j => j.ValidDiscountRateVr))
                .ForMember(dest => dest.uzm_validdiscountratewcol , from => from.MapFrom(j => j.ValidDiscountRateWcol))
                .ForMember(dest => dest.uzm_wagescaleidvakko, from => from.MapFrom(j => j.WageScaleIdVakko))
                .ForMember(dest => dest.uzm_wagescaleidvr, from => from.MapFrom(j => j.WageScaleIdVr))
                .ForMember(dest => dest.uzm_wagescaleidwcol, from => from.MapFrom(j => j.WageScaleIdWcol))
                .ReverseMap();


        }
    }
}
