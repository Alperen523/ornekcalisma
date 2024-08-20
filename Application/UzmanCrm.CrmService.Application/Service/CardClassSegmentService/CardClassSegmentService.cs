using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.LoyaltyCardClassSegment;

namespace UzmanCrm.CrmService.Application.Service.CardClassSegmentService
{
    public class CardClassSegmentService : ICardClassSegmentService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly ICRMService crmService;

        public CardClassSegmentService(IMapper mapper,
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
        /// CardClass segment save for CRM
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<CardClassSegmentSaveResponseDto>> CardClassSegmentSaveAsync(CardClassSegmentRequestDto requestDto)
        {
            var responseModel = new Response<CardClassSegmentSaveResponseDto>();

            var modelDto = mapper.Map<CardClassSegmentDto>(requestDto);
            try
            {
                var resService = await GetCardClassSegmentItemAsync(requestDto);
                if (resService.Success)
                {
                    modelDto.uzm_cardclasssegmentid = resService.Data.uzm_cardclasssegmentid;
                }

                var entityModel = mapper.Map<CardClassSegment>(modelDto);
                entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);

                var result = crmService.Save<CardClassSegment>(entityModel, "uzm_cardclasssegment", "uzm_cardclasssegment", Common.Enums.CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(CardClassSegmentSaveAsync),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<CardClassSegmentSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.CardClassSegmentSaveError + ex.ToString(), "")));
            }

            return responseModel;
        }

        private async Task<Response<CardClassSegmentDto>> GetCardClassSegmentItemAsync(CardClassSegmentRequestDto requestDto)
        {
            var query = String.Format(@"select uzm_cardclasssegmentId from uzm_cardclasssegmentBase with(nolock) where uzm_name=@SegmentName and statecode=0");
            var resService = await dapperService.GetItemParam<object, CardClassSegmentDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;
        }

        /// <summary>
        /// Get Card Class Segment List
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<List<CardClassSegmentDto>>> GetCardClassSegmentListAsync(string SegmentName)
        {
            var model = new Response<List<CardClassSegmentDto>>();

            var getCardClassSegmentListResponse = await CardClassSegmentGetList(SegmentName);
            if (getCardClassSegmentListResponse.Success)
            {
                model.Success = true;
                model.Message = CommonStaticConsts.Message.Success;
                model.Data = getCardClassSegmentListResponse.Data;
            }
            else
            {
                model.Data = null;
                model.Success = false;
                model.Error = getCardClassSegmentListResponse.Error != null ? getCardClassSegmentListResponse.Error :
                    (new ErrorModel { Description = CommonStaticConsts.Message.CardClassSegmentListGetError, StatusCode = System.Net.HttpStatusCode.InternalServerError, ErrorCode = ErrorStaticConsts.CardClassSegmentStaticConsts.CCS0001 });
                model.Message = getCardClassSegmentListResponse.Message;
                return model;
            }

            return model;
        }

        private async Task<Response<List<CardClassSegmentDto>>> CardClassSegmentGetList(string SegmentName)
        {
            var whereStatement = "";
            if (SegmentName.IsNotNullAndEmpty())
                whereStatement = $"uzm_name = '{SegmentName}' AND";

            var queryCardClassSegment = @$"
SELECT
	uzm_cardclasssegmentId,
	uzm_name,
	uzm_description,
	uzm_validityperiod,
	uzm_notificationperiod,
    uzm_secondnotificationperiod
FROM uzm_cardclasssegment with(nolock) where {whereStatement} statecode=0";
            return await dapperService.GetListByParamAsync<object, CardClassSegmentDto>(queryCardClassSegment, null, GeneralHelper.GetCrmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }
    }
}
