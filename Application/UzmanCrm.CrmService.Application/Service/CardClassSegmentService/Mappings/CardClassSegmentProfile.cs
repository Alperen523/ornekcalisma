using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService.Model;
using UzmanCrm.CrmService.Domain.Entity.CRM.LoyaltyCardClassSegment;

namespace UzmanCrm.CrmService.Application.Service.CardClassSegmentService.Mappings
{
    public class CardClassSegmentProfile : Profile
    {
        public CardClassSegmentProfile()
        {
            this.CreateMap<CardClassSegmentDto, CardClassSegment>().ReverseMap();
            //this.CreateMap<Response<EmailDto>, Response<Email>>().ReverseMap();
            //this.CreateMap<Response<List<EmailDto>>, Response<List<Email>>>().ReverseMap();

            //this.CreateMap<EmailDto, ContactEmailDto>()
            //    .ForMember(_ => _.Email, i => i.MapFrom(j => j.uzm_emailaddress))
            //    .ForMember(_ => _.Permission, i => i.MapFrom(j => j.uzm_emailpermission))
            //    .ForMember(_ => _.CreatedDate, i => i.MapFrom(j => j.createdon))
            //    .ForMember(_ => _.UpdatedDate, i => i.MapFrom(j => j.modifiedon))
            //    .ForMember(_ => _.CreatedPerson, i => i.MapFrom(j => j.createdpersonno))
            //    .ForMember(_ => _.UpdatedPerson, i => i.MapFrom(j => j.updatedpersonno))
            //    .ReverseMap();

            //this.CreateMap<Response<EmailDto>, Response<ContactEmailDto>>().ReverseMap();
            //this.CreateMap<Response<List<EmailDto>>, Response<List<ContactEmailDto>>>().ReverseMap();

            this.CreateMap<CardClassSegmentRequestDto, CardClassSegmentDto>()
                .ForMember(_ => _.uzm_description, i => i.MapFrom(j => j.Description))
                .ForMember(_ => _.uzm_name, i => i.MapFrom(j => j.SegmentName))
                .ForMember(_ => _.uzm_validityperiod, i => i.MapFrom(j => j.ValidityPeriod))
                .ForMember(_ => _.uzm_notificationperiod, i => i.MapFrom(j => j.FirstNotificationPeriod))
                .ForMember(_ => _.uzm_secondnotificationperiod, i => i.MapFrom(j => j.SecondNotificationPeriod))
                .ReverseMap();
        }
    }
}
