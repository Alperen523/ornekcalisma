using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            CreateMap(typeof(Response<>), typeof(ResponseApi<>)).ReverseMap();

            this.CreateMap<GetCustomerRequest, GetCustomerRequestDto>().ReverseMap();
            this.CreateMap<GetCustomerResponseDto, GetCustomerResponse>().ReverseMap();
            this.CreateMap<Response<GetCustomerResponseDto>, Response<GetCustomerResponse>>().ReverseMap();

            this.CreateMap<SaveCustomerRequest, SaveCustomerRequestDto>()
                .ForMember(_ => _.CrmId, i => i.MapFrom(j => j.CrmId == System.Guid.Empty ? null : j.CrmId))
                .ReverseMap();

            this.CreateMap<SaveCustomerResponseDto, SaveCustomerResponse>().ReverseMap();
            this.CreateMap<Response<SaveCustomerResponseDto>, Response<SaveCustomerResponse>>().ReverseMap();








        }
    }
}