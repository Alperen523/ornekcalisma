using AutoMapper;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.CardClassSegment;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class CardClassSegmentProfile : Profile
    {
        public CardClassSegmentProfile()
        {
            this.CreateMap<CardClassSegmentRequest, CardClassSegmentRequestDto>().ReverseMap();

            this.CreateMap<CardClassSegmentSaveResponseDto, CardClassSegmentSaveResponse>().ReverseMap();
            this.CreateMap<Response<CardClassSegmentSaveResponseDto>, Response<CardClassSegmentSaveResponse>>().ReverseMap();

            this.CreateMap<CardClassSegmentDto, CardClassSegmentGetResponse>()
                .ForMember(dest => dest.Id, from => from.MapFrom(j => j.uzm_cardclasssegmentid))
                .ForMember(dest => dest.ValidityPeriod, from => from.MapFrom(j => j.uzm_validityperiod))
                .ForMember(dest => dest.SegmentName, from => from.MapFrom(j => j.uzm_name))
                .ForMember(dest => dest.Description, from => from.MapFrom(j => j.uzm_description))
                .ForMember(dest => dest.FirstNotificationPeriod, from => from.MapFrom(j => j.uzm_notificationperiod))
                .ForMember(dest => dest.SecondNotificationPeriod, from => from.MapFrom(j => j.uzm_secondnotificationperiod))
                .ReverseMap();

            this.CreateMap<Response<List<CardClassSegmentDto>>, Response<List<CardClassSegmentGetResponse>>>().ReverseMap();
        }
    }
}