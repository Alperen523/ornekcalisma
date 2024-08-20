using AutoMapper;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Models;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Domain.Entity.CRM.Adress;

namespace UzmanCrm.CrmService.Application.Service.AddressService.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            this.CreateMap<Country, CountryDto>().ReverseMap();
            this.CreateMap<Response<Country>, Response<CountryDto>>().ReverseMap();
            this.CreateMap<Response<List<Country>>, Response<List<CountryDto>>>().ReverseMap();

            this.CreateMap<City, CityDto>().ReverseMap();
            this.CreateMap<Response<City>, Response<CityDto>>().ReverseMap();
            this.CreateMap<Response<List<City>>, Response<List<CityDto>>>().ReverseMap();

            this.CreateMap<District, DistrictDto>().ReverseMap();
            this.CreateMap<Response<District>, Response<DistrictDto>>().ReverseMap();
            this.CreateMap<Response<List<District>>, Response<List<DistrictDto>>>().ReverseMap();

            this.CreateMap<Neighborhood, NeighborhoodDto>().ReverseMap();
            this.CreateMap<Response<Neighborhood>, Response<NeighborhoodDto>>().ReverseMap();
            this.CreateMap<Response<List<Neighborhood>>, Response<List<NeighborhoodDto>>>().ReverseMap();

            this.CreateMap<Address, AddressDto>().ReverseMap();
            this.CreateMap<AddressSaveRequestDto, AddressDto>()
               .ForMember(_ => _.uzm_customerid, i => i.MapFrom(j => j.CustomerCrmId))
               .ForMember(_ => _.uzm_countryid, i => i.MapFrom(j => j.CountryId))
               .ForMember(_ => _.uzm_cityid, i => i.MapFrom(j => j.CityId))
               .ForMember(_ => _.uzm_districtid, i => i.MapFrom(j => j.DistrictId))
               .ForMember(_ => _.uzm_neighborhoodid, i => i.MapFrom(j => j.NeighborhoodId))
               .ForMember(_ => _.uzm_modifiedbylocationid, i => i.MapFrom(j => j.LocationId))
               .ForMember(_ => _.uzm_createdlocationid, i => i.MapFrom(j => j.LocationId))
               .ForMember(_ => _.uzm_modifiedbypersonid, i => i.MapFrom(j => j.PersonId))
               .ForMember(_ => _.uzm_createdbypersonid, i => i.MapFrom(j => j.PersonId))
               .ForMember(_ => _.uzm_fulladdress, i => i.MapFrom(j => j.AddressLine))
               .ForMember(_ => _.uzm_datasourceid, i => i.MapFrom(j => j.ChannelId))
               .ForMember(_ => _.uzm_addressecomidstr, i => i.MapFrom(j => j.AddressId))
               .ForMember(_ => _.uzm_postcode, i => i.MapFrom(j => j.PostCode))
               .ForMember(_ => _.uzm_isdefaultaddress, i => i.MapFrom(j => j.IsDefaultAddress))
               .ForMember(_ => _.uzm_addresstype, i => i.MapFrom(j => j.AddressType))
               .ReverseMap();

            this.CreateMap<DeleteAddressRequestDto, AddressDto>()
               .ForMember(_ => _.uzm_customerid, i => i.MapFrom(j => j.CustomerCrmId))
               .ForMember(_ => _.uzm_modifiedbylocationid, i => i.MapFrom(j => j.LocationId))
               .ForMember(_ => _.uzm_createdlocationid, i => i.MapFrom(j => j.LocationId))
               .ForMember(_ => _.uzm_modifiedbypersonid, i => i.MapFrom(j => j.PersonId))
               .ForMember(_ => _.uzm_createdbypersonid, i => i.MapFrom(j => j.PersonId))
               .ForMember(_ => _.uzm_addressecomidstr, i => i.MapFrom(j => j.AddressId))
               .ForMember(_ => _.uzm_customeraddressid, i => i.MapFrom(j => j.AddressCrmId))
               .ReverseMap();

        }
    }
}
