using AutoMapper;
using System;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;

namespace UzmanCrm.CrmService.Application.Service.CardTypeService
{
    public class CardTypeService : ICardTypeService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly ICRMService crmService;

        public CardTypeService(IMapper mapper,
            ICRMService crmService,
            IDapperService dapperService,
            ILogService logService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
            this.crmService = crmService;
        }

        public async Task<Response<CardTypeDto>> GetCardTypeByNameItemAsync(CardTypeEnum cardTypeName)
        {
            var query = String.Format($@"
SELECT uzm_cardtypedefinitionId, createdon, modifiedon, statecode, statuscode, uzm_name, uzm_cardtypedescription, uzm_code
FROM uzm_cardtypedefinition WITH(NOLOCK)
WHERE uzm_name='{cardTypeName}' and statecode=0");
            var resService = await dapperService.GetItemParam<object, CardTypeDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        public async Task<Response<CardTypeDto>> GetCardTypeByCodeItemAsync(string cardCode)
        {
            var query = String.Format($@"
SELECT uzm_cardtypedefinitionId, createdon, modifiedon, statecode, statuscode, uzm_name, uzm_cardtypedescription, uzm_code
FROM uzm_cardtypedefinition WITH(NOLOCK)
WHERE uzm_code='{cardCode}' and statecode=0");
            var resService = await dapperService.GetItemParam<object, CardTypeDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        public async Task<Response<CardTypeDto>> GetCardTypeByIdItemAsync(string cardTypeId)
        {
            var query = String.Format($@"
SELECT uzm_cardtypedefinitionId, createdon, modifiedon, statecode, statuscode, uzm_name, uzm_cardtypedescription, uzm_code
FROM uzm_cardtypedefinition WITH(NOLOCK)
WHERE uzm_cardtypedefinitionId='{cardTypeId}' and statecode=0");
            var resService = await dapperService.GetItemParam<object, CardTypeDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }
    }
}
