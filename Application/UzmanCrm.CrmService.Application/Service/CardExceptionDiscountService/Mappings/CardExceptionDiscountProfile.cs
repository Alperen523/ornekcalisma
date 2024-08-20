using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Domain.Entity.CRM.CardExceptionDiscount;

namespace UzmanCrm.CrmService.Application.Service.CardExceptionDiscountService.Mappings
{
    public class CardExceptionDiscountProfile : Profile
    {
        public CardExceptionDiscountProfile()
        {
            this.CreateMap<CardExceptionDiscountDto, CardExceptionDiscount>()
                .ReverseMap();

            this.CreateMap<CardExceptionDiscountRequestDto, CardExceptionDiscountDto>()
                .ForMember(dest => dest.uzm_approvalstatus, from => from.MapFrom(j => j.ApprovalStatus))
                .ForMember(_ => _.uzm_approvedby, i => i.MapFrom(j => j.ApprovedByUserId))
                .ForMember(_ => _.uzm_cardclasssegmentid, i => i.MapFrom(j => j.CardClassSegmentId))
                .ForMember(_ => _.uzm_customergroupid, i => i.MapFrom(j => j.CustomerGroupId))
                .ForMember(_ => _.uzm_demanddate, i => i.MapFrom(j => j.DemandDate))
                .ForMember(_ => _.uzm_demandeduser, i => i.MapFrom(j => j.DemandedUserId))
                .ForMember(_ => _.uzm_demanddate, i => i.MapFrom(j => j.DemandDate))
                .ForMember(_ => _.uzm_description, i => i.MapFrom(j => j.Description))
                .ForMember(_ => _.uzm_discountrate, i => i.MapFrom(j => j.DiscountRate))
                .ForMember(_ => _.uzm_enddate, i => i.MapFrom(j => j.EndDate))
                .ForMember(_ => _.uzm_loyaltycardid, i => i.MapFrom(j => j.LoyaltyCardId))
                .ForMember(_ => _.uzm_startdate, i => i.MapFrom(j => j.StartDate))
                .ForMember(_ => _.uzm_statuscode, i => i.MapFrom(j => j.StatusCode))
                .ForMember(_ => _.uzm_approvalexplanation, i => i.MapFrom(j => j.ApprovalExplanation))
                .ForMember(_ => _.uzm_arrivalchannel, i => i.MapFrom(j => j.ArrivalChannel))
                .ForMember(_ => _.uzm_demandstore, i => i.MapFrom(j => j.DemandStore))
                .ForMember(_ => _.uzm_carddiscountId, i => i.MapFrom(j => j.CardDiscountId))
                .ReverseMap();

            this.CreateMap<CardExceptionDiscountRequestDto, CardExceptionDiscountAndContactDto>()
                .ForMember(dest => dest.uzm_approvalstatus, from => from.MapFrom(j => j.ApprovalStatus))
                .ForMember(_ => _.uzm_approvedby, i => i.MapFrom(j => j.ApprovedByUserId))
                //.ForMember(_ => _.uzm_cardclasssegmentId, i => i.MapFrom(j => j.CardClassSegmentId))
                //.ForMember(_ => _.uzm_customergroupId, i => i.MapFrom(j => j.CustomerGroupId))
                .ForMember(_ => _.uzm_demanddate, i => i.MapFrom(j => j.DemandDate))
                .ForMember(_ => _.uzm_demandeduser, i => i.MapFrom(j => j.DemandedUserId))
                .ForMember(_ => _.uzm_demanddate, i => i.MapFrom(j => j.DemandDate))
                .ForMember(_ => _.uzm_description, i => i.MapFrom(j => j.Description))
                .ForMember(_ => _.uzm_discountrate, i => i.MapFrom(j => j.DiscountRate))
                .ForMember(_ => _.uzm_enddate, i => i.MapFrom(j => j.EndDate))
                //.ForMember(_ => _.uzm_loyaltycardId, i => i.MapFrom(j => j.LoyaltyCardId))
                .ForMember(_ => _.uzm_loyaltycardid, i => i.MapFrom(j => j.LoyaltyCardId))
                .ForMember(_ => _.uzm_startdate, i => i.MapFrom(j => j.StartDate))
                .ForMember(_ => _.uzm_statuscode, i => i.MapFrom(j => j.StatusCode))
                .ReverseMap();

            this.CreateMap<CardApprovalStatusAndExplanationRequestDto, CardExceptionDiscount>()
                .ForMember(dest => dest.uzm_approvalstatus, from => from.MapFrom(j => j.ApprovalStatus))
                .ForMember(dest => dest.uzm_carddiscountid, from => from.MapFrom(j => j.CardDiscountId))
                .ForMember(dest => dest.uzm_loyaltycardid, from => from.MapFrom(j => j.LoyaltyCardId))
                .ForMember(dest => dest.uzm_statuscode, from => from.MapFrom(j => j.StatusCode))
                .ForMember(dest => dest.uzm_approvalexplanation, from => from.MapFrom(j => j.ApprovalExplanation))
                .ForMember(dest => dest.uzm_arrivalchannel, from => from.MapFrom(j => j.ArrivalChannel))
                .ForMember(dest => dest.uzm_demandstore, from => from.MapFrom(j => j.BusinessUnitId))
                .ReverseMap();

            this.CreateMap<CardEndDateRequestDto, CardExceptionDiscount>()
                .ForMember(dest => dest.uzm_enddate, from => from.MapFrom(j => j.EndDate))
                .ForMember(dest => dest.uzm_discountrate, from => from.MapFrom(j => j.DiscountRate))
                .ForMember(dest => dest.uzm_carddiscountid, from => from.MapFrom(j => j.CardDiscountId))
                .ReverseMap();
        }
    }
}
