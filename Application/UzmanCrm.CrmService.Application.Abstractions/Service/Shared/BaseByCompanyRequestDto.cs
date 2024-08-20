using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.Shared
{
    public class BaseByCompanyRequestDto
    {
        public CompanyEnum Company { get; set; } = CompanyEnum.KD;
    }
}
