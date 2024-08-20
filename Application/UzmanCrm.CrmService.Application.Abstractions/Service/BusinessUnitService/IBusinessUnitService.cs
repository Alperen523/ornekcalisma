using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService
{
    public interface IBusinessUnitService : IApplicationService
    {
        Task<Response<StoreDto>> GetStoreCodeAsync(string storeCode, CompanyEnum company);

        Task<Response<List<BusinessUnitDto>>> GetBusinessUnitListAsync(CompanyEnum company);

        Task<Response<BusinessUnitDto>> GetBusinessUnitById(Guid id, CompanyEnum company);
    }
}
