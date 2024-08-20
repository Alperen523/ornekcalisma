using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;

namespace UzmanCrm.CrmService.Application.Service.CustomerGroupService
{
    public class CustomerGroupService : ICustomerGroupService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;

        public CustomerGroupService(IMapper mapper,
            IDapperService dapperService,
            ILogService logService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
        }

        /// <summary>
        /// Get Customer Group List
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<List<CustomerGroupGetDto>>> GetCustomerGroupListAsync(string CustomerGroupCode)
        {
            var model = new Response<List<CustomerGroupGetDto>>();

            var getCustomerGroupListResponse = await CustomerGroupGetList(CustomerGroupCode);
            if (getCustomerGroupListResponse.Success)
            {
                model.Success = true;
                model.Message = CommonStaticConsts.Message.Success;
                model.Data = getCustomerGroupListResponse.Data;
            }
            else
            {
                model.Data = null;
                model.Success = false;
                model.Error = getCustomerGroupListResponse.Error != null ? getCustomerGroupListResponse.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.CustomerGroupListGetError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.CustomerGroupStaticConsts.CG0001 });
                model.Message = getCustomerGroupListResponse.Message;
                return model;
            }

            return model;
        }

        private async Task<Response<List<CustomerGroupGetDto>>> CustomerGroupGetList(string CustomerGroupCode)
        {
            var whereStatement = "";
            if (CustomerGroupCode.IsNotNullAndEmpty())
                whereStatement = $"uzm_groupcode = '{CustomerGroupCode}' AND";

            var queryCustomerGroup = @$"SELECT uzm_customergroupId, uzm_name, uzm_groupcode
FROM uzm_customergroup with(nolock) where {whereStatement} statecode=0";
            return await dapperService.GetListByParamAsync<object, CustomerGroupGetDto>(queryCustomerGroup, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }
    }
}
