using AutoMapper;
using System;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService.Model;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.User;

namespace UzmanCrm.CrmService.Application.Service.UserService
{
    public class UserService : IUserService
    {

        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;

        public UserService(IMapper mapper,
            IDapperService dapperService,
            ILogService logService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
        }

        public async Task<Response<EmployeeDto>> GetEmployeeNumberAsync(string registrationNumber, CompanyEnum company)
        {
            var query = String.Format(@"SELECT [uzm_employeeId]
                                          ,[CreatedOn]
                                          ,[CreatedBy]
                                          ,[ModifiedOn]
                                          ,[ModifiedBy]
                                          ,[CreatedOnBehalfBy]
                                          ,[ModifiedOnBehalfBy]
                                          ,[OwnerId]
                                          ,[OwnerIdType]
                                          ,[OwningBusinessUnit]
                                          ,[statecode]
                                          ,[statuscode]
                                          ,[VersionNumber]
                                          ,[ImportSequenceNumber]
                                          ,[OverriddenCreatedOn]
                                          ,[TimeZoneRuleVersionNumber]
                                          ,[UTCConversionTimeZoneCode]
                                          ,[uzm_name]
                                          ,[uzm_lastname]
                                          ,[uzm_registrationnumber]
                                          ,[uzm_workingarea]
                                          ,[uzm_storeid]
                                          ,[uzm_email]
                                          ,[uzm_mobilephone]
                                          ,[uzm_workstartdate]
                                          ,[uzm_dimissaldate]
                                          ,[uzm_tcidentificationnumber]
                                          ,[uzm_organizationid]
                                          ,[uzm_departmentid]
                                          ,[uzm_workingstatus]
                                          ,[uzm_integrationuser]
                                      FROM [uzm_employeeBase] with(nolock)
                                      WHERE
                                            statecode = 0 AND
                                            uzm_registrationnumber = @registrationNumber");

            var resService = await dapperService.GetItemParam<object, Employee>(query, new { registrationNumber = registrationNumber }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<EmployeeDto>>(resService);

            return response;
        }
    }
}
