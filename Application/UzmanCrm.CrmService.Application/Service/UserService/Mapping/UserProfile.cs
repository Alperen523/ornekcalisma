using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService.Model;
using UzmanCrm.CrmService.Domain.Entity.CRM.User;

namespace UzmanCrm.CrmService.Application.Service.UserService.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            this.CreateMap<Employee, EmployeeDto>().ReverseMap();

            this.CreateMap<Response<EmployeeDto>, Response<Employee>>().ReverseMap();
        }
    }
}
