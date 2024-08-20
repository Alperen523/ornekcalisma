using AutoMapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Login;
using UzmanCrm.CrmService.WebAPI.Models.Login;

namespace UzmanCrm.CrmService.WebAPI.Mappings
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            this.CreateMap<ApiUserLoginRequestDto, LoginRequest>()
                .ReverseMap();
        }
    }
}