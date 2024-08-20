using AutoMapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Login;

namespace UzmanCrm.CrmService.Application.Service.LoginService.Mapping
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            this.CreateMap<ApiUserLoginRequestDto, ApiUserLogin>()
                .ForMember(_ => _.uzm_username, i => i.MapFrom(j => j.Username))
                .ForMember(_ => _.uzm_password, i => i.MapFrom(j => j.Password))
                .ReverseMap()
                .ForMember(_ => _.Username, i => i.MapFrom(j => j.uzm_username))
                .ForMember(_ => _.Password, i => i.MapFrom(j => j.uzm_password));



        }
    }
}
