using AutoMapper;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Domain.Entity.CRM.BusinessUnit;

namespace UzmanCrm.CrmService.Application.Service.BusinessUnitService.Mappings
{
    public class BusinessUnitProfile : Profile
    {
        public BusinessUnitProfile()
        {
            this.CreateMap<Store, StoreDto>().ReverseMap();
            this.CreateMap<Response<Store>, Response<StoreDto>>().ReverseMap();

            //old..
            this.CreateMap<BusinessUnit, BusinessUnitDto>().ReverseMap();
            this.CreateMap<Response<BusinessUnit>, Response<BusinessUnitDto>>().ReverseMap();
            this.CreateMap<Response<List<BusinessUnit>>, Response<List<BusinessUnitDto>>>().ReverseMap();
        }
    }
}
