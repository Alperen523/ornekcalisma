using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList;
using Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList.Model;

namespace UzmanCrm.CrmService.Application.Service.BatchApprovalList
{
    public class BatchApprovalListService : IBatchApprovalListService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly ICRMService crmService;

        public BatchApprovalListService(IMapper mapper,
            ICRMService crmService,
            IDapperService dapperService,
            ILogService logService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.logService = logService;
            this.crmService = crmService;
        }

        /// <summary>
        /// İşleme alınacak Toplu Onay Listesi kayıtlarını listeler. Süreç durumu = "Devam Ediyor", İsitsna Oany Durumu = "Reddedildi" veya "Onaylandı"
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<BatchApprovalListDto>>> GetWillBeProcessedBatchApprovalList()
        {
            var query = @$"SELECT *
                           FROM [KahveDunyasi_MSCRM].[dbo].[Filteredvkk_batchapprovallist] WITH(NOLOCK)
                           WHERE statecode=0 AND vkk_processstatus=0 AND (vkk_approvalstatus in (2,3))";
            return await dapperService.GetListByParamAsync<object, BatchApprovalListDto>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        public async Task<Response<BatchApprovalListResponseDto>> UpdateBatchApprovalListProcessStatusAsync(BatchApprovalListProcessStatusRequestDto requestDto)
        {
            var responseModel = new Response<BatchApprovalListResponseDto>();
            try
            {
                var cardException = mapper.Map<BatchApprovalListDto>(requestDto);

                var result = crmService.Save(cardException, "vkk_batchapprovallist", "vkk_batchapprovallist", CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(UpdateBatchApprovalListProcessStatusAsync),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<BatchApprovalListResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.BatchApprovalListSaveError + ex.ToString(), "")));
            }

            return responseModel;
        }
    }
}
