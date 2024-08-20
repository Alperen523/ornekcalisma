using AutoMapper;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.CustomerGroup;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class CustomerGroupProfile : Profile
    {
        public CustomerGroupProfile()
        {
            this.CreateMap<CustomerGroupGetDto, CustomerGroupGetResponse>()
                .ForMember(dest => dest.Id, from => from.MapFrom(j => j.uzm_customergroupid))
                .ForMember(dest => dest.CustomerGroupName, from => from.MapFrom(j => j.uzm_name))
                .ForMember(dest => dest.Code, from => from.MapFrom(j => j.uzm_groupcode))
                .ReverseMap();

            this.CreateMap<Response<List<CustomerGroupGetDto>>, Response<List<CustomerGroupGetResponse>>>().ReverseMap();
        }
    }
}