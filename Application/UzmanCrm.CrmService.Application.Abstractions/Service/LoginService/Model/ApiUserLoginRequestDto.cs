using System.Collections.Generic;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Domain.Entity.CRM.Login
{

    public class ApiUserLoginRequestDto
    {
        public ApiUserLoginRequestDto()
        {
            Roles = new List<RoleDto>();
        }
        public string Username { get; set; }

        public string Password { get; set; }
        public List<RoleDto> Roles { get; set; }
        public CompanyEnum Company { get; set; } = CompanyEnum.KD;

    }
    public class RoleDto
    {
        public RoleDto(string name)
        {
            RoleName = name;
        }

        public string RoleName { get; set; }
    }
}
