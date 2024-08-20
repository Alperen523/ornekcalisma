using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.WebAPI.Models.Address;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            this.CreateMap<AddressSaveRequest, AddressSaveRequestDto>()
               .ForMember(_ => _.PostCode, i => i.MapFrom(j => GeneralHelper.FieldNullReplace(j.PostCode)))
               .ForMember(_ => _.AddressLine, i => i.MapFrom(j => GeneralHelper.FieldNullReplace(j.AddressLine)))
               .ReverseMap();

            this.CreateMap<DeleteAddressRequest, DeleteAddressRequestDto>().ReverseMap();

        }
    }
}