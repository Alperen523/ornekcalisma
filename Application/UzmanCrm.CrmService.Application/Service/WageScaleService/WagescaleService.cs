using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService;
using UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.WageScaleService.Model;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Wagescale;

namespace UzmanCrm.CrmService.Application.Service.WagescaleService
{
    public class WageScaleService : IWageScaleService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly ILogService logService;
        private readonly ICRMService crmService;

        public WageScaleService(IMapper mapper,
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
        /// Şu an itibariyle şimdiden küçük en güncel üst barem kayıtlarını temp tabloya atar <br/>
        /// Sonrasında bu tablodaki id'ler ile ilişkili alt barem kayıtlarını listeler
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<ValidWageScaleListDto>>> ValidWageScaleGetList()
        {
            var queryWagescale = @$"
SELECT [uzm_wagescaleId]
      ,[uzm_rangestart]
      ,[uzm_rangeend]
      ,[uzm_discountrate]
      ,[uzm_cardtypedefinitionid]
  FROM [OVM].[dbo].[ValidWageScaleList]
";
            return await dapperService.GetListByParamAsync<object, ValidWageScaleListDto>(queryWagescale, null, GeneralHelper.GetOvmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }

        /// <summary>
        /// Create customer wagescale for CRM
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<Response<CustomerWageScaleResponseDto>> CreateCustomerWagescaleAsync(CreateCustomerWageScaleRequestDto requestDto)
        {
            var responseModel = new Response<CustomerWageScaleResponseDto>();

            var modelDto = mapper.Map<CustomerWageScaleDto>(requestDto);
            try
            {

                var entityModel = mapper.Map<CustomerWageScale>(modelDto);


                var result = crmService.Save<CustomerWageScale>(entityModel, "uzm_customerwagescale", "uzm_customerwagescale", Common.Enums.CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;
                responseModel.Error = result.Error;
            }
            catch (Exception ex)
            {
                await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(CreateCustomerWagescaleAsync),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );

                return await Task.FromResult(ResponseHelper.SetSingleError<CustomerWageScaleResponseDto>(new ErrorModel(System.Net.HttpStatusCode.BadRequest,
                    CommonStaticConsts.Message.CardClassSegmentSaveError + ex.ToString(), "")));
            }

            return responseModel;
        }

        /// <summary>
        /// Kart tipine göre barem aralığı bulma metodudur
        /// </summary>
        /// <param name="wagescaleList">Barem Listesi</param>
        /// <param name="validEndorsement">Geçerli Ciro</param>
        /// <param name="cardTypeId">Sorgulanmak istenilen kart tipi</param>
        /// <returns></returns>
        public async Task<Response<ValidWageScaleListDto>> GetWagescaleByEndorsement(List<ValidWageScaleListDto> wagescaleList, double validEndorsement, Guid cardTypeId)
        {
            var result = new Response<ValidWageScaleListDto>();
            try
            {
                var selectedWagescaleList = (from item in wagescaleList
                                             where (item.uzm_rangestart <= validEndorsement) && (item.uzm_rangeend > validEndorsement) && (item.uzm_cardtypedefinitionid == cardTypeId)
                                             select item).ToList();
                if (selectedWagescaleList.Count > 0)
                {
                    result.Success = true;
                    result.Message = CommonStaticConsts.Message.Success;
                    result.Data = selectedWagescaleList.FirstOrDefault();
                }
                else
                {
                    result.Message = CommonStaticConsts.Message.CustomerEndorsementDataProcessingWagescaleSelectError;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Vakko tipli kart için: istenilen kart tipi ne olursa olsun Vakko baremlerinden sorgular
        /// Vakkorama tipli kart için: istenilen kart tipi ne olursa olsun Vakkorama baremlerinden sorgular
        /// W Collection tipli kart için: istenilen kart tipi ne ise onun kendi barem listesinden sorgular
        /// </summary>
        /// <param name="WagescaleList">Barem Listesi</param>
        /// <param name="ValidEndorsement">Geçerli Ciro</param>
        /// <param name="CardType">Sorgulanmak istenen kart tipi parametresi</param>
        /// <param name="cardTypeDefinitionId"></param>
        /// <param name="cardTypeEnum">Loyalty Card kaydının kendi kart tipi bilgisidir</param>
        /// <returns></returns>
        public async Task<Response<ValidWageScaleListDto>> GetValidWagescale(List<ValidWageScaleListDto> WagescaleList, double ValidEndorsement, CardTypeEnum CardType, Guid cardTypeDefinitionId, CardTypeEnum cardTypeEnum)
        {
            var result = new Response<ValidWageScaleListDto>();
            var cardTypeId = GeneralHelper.GetCardIdByCardTypeEnum(CardType);

            switch (CardType)
            {
                case CardTypeEnum.V:
                    result = await GetWagescaleByEndorsement(WagescaleList, ValidEndorsement, cardTypeId);
                    break;
                case CardTypeEnum.R:
                    result = await GetWagescaleByEndorsement(WagescaleList, ValidEndorsement, cardTypeId);
                    break;
                case CardTypeEnum.W:
                    result = await GetWagescaleByEndorsement(WagescaleList, ValidEndorsement, cardTypeDefinitionId);
                    break;
                default:
                    break;
            }

            return result;
        }


        /// <summary>
        /// Kart tipine göre en düşük barem bulma metodudur
        /// </summary>
        /// <param name="wagescaleList">Barem Listesi</param>
        /// <param name="cardTypeId">Sorgulanmak istenilen kart tipi</param>
        /// <returns></returns>
        public async Task<Response<ValidWageScaleListDto>> GetMinWagescaleByCardType(List<ValidWageScaleListDto> wagescaleList, Guid cardTypeId)
        {
            var result = new Response<ValidWageScaleListDto>();
            try
            {
                var selectedWagescaleList = (from item in wagescaleList
                                             where (item.uzm_cardtypedefinitionid == cardTypeId)
                                             select item).OrderBy(x => x.uzm_discountrate).ToList();
                if (selectedWagescaleList.Count > 0)
                {
                    result.Success = true;
                    result.Message = CommonStaticConsts.Message.Success;
                    result.Data = selectedWagescaleList.FirstOrDefault();
                }
                else
                {
                    result.Message = CommonStaticConsts.Message.CustomerEndorsementDataProcessingWagescaleSelectError;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Kart tipine göre indirim oranı 0'dan büyük en düşük barem bulma metodudur
        /// </summary>
        /// <param name="wagescaleList">Barem Listesi</param>
        /// <param name="cardTypeId">Sorgulanmak istenilen kart tipi</param>
        /// <returns></returns>
        public Response<ValidWageScaleListDto> GetBiggerThanZeroDiscountMinWagescaleByCardType(List<ValidWageScaleListDto> wagescaleList, Guid cardTypeId)
        {
            var result = new Response<ValidWageScaleListDto>();
            try
            {
                var selectedWagescaleList = (from item in wagescaleList
                                             where (item.uzm_cardtypedefinitionid == cardTypeId & item.uzm_discountrate > 0)
                                             select item).OrderBy(x => x.uzm_discountrate).ToList();
                if (selectedWagescaleList.Count > 0)
                {
                    result.Success = true;
                    result.Message = CommonStaticConsts.Message.Success;
                    result.Data = selectedWagescaleList.FirstOrDefault();
                }
                else
                {
                    result.Message = CommonStaticConsts.Message.CustomerEndorsementDataProcessingWagescaleSelectError;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Kart tipine göre barem alt sınırı geçerli cirodan büyük en düşük barem bulma metodudur
        /// </summary>
        /// <param name="wagescaleList">Barem Listesi</param>
        /// <param name="cardTypeId">Sorgulanmak istenilen kart tipi</param>
        /// <param name="validEndorsement">Geçerli Ciro</param>
        /// <returns></returns>
        public async Task<Response<ValidWageScaleListDto>> GetBiggerThanValidEndorsementMinWagescaleByCardType(List<ValidWageScaleListDto> wagescaleList, double validEndorsement, Guid cardTypeId)
        {
            var result = new Response<ValidWageScaleListDto>();
            try
            {
                var selectedWagescaleList = (from item in wagescaleList
                                             where (item.uzm_cardtypedefinitionid == cardTypeId & item.uzm_rangestart > validEndorsement)
                                             select item).OrderBy(x => x.uzm_discountrate).ToList();
                if (selectedWagescaleList.Count > 0)
                {
                    result.Success = true;
                    result.Message = CommonStaticConsts.Message.Success;
                    result.Data = selectedWagescaleList.FirstOrDefault();
                }
                else
                {
                    result.Message = CommonStaticConsts.Message.CustomerEndorsementDataProcessingWagescaleSelectError;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Kart tipine göre barem alt sınırı geçerli cirodan büyük en düşük barem bulma metodudur:
        /// Vakko tipli kart için: istenilen kart tipi ne olursa olsun Vakko baremlerinden sorgular
        /// Vakkorama tipli kart için: istenilen kart tipi ne olursa olsun Vakkorama baremlerinden sorgular
        /// W Collection tipli kart için: istenilen kart tipi ne ise onun kendi barem listesinden sorgular
        /// </summary>
        /// <param name="WagescaleList">Barem Listesi</param>
        /// <param name="ValidEndorsement">Geçerli Ciro</param>
        /// <param name="CardType">Sorgulanmak istenen kart tipi parametresi</param>
        /// <param name="cardTypeDefinitionId"></param>
        /// <param name="cardTypeEnum">Loyalty Card kaydının kendi kart tipi bilgisidir</param>
        /// <returns></returns>
        public async Task<Response<ValidWageScaleListDto>> GetValidUpperWagescale(List<ValidWageScaleListDto> WagescaleList, double ValidEndorsement, CardTypeEnum CardType, Guid cardTypeDefinitionId, CardTypeEnum cardTypeEnum)
        {
            var result = new Response<ValidWageScaleListDto>();
            var cardTypeId = GeneralHelper.GetCardIdByCardTypeEnum(CardType);
            switch (CardType)
            {
                case CardTypeEnum.V:
                    result = await GetBiggerThanValidEndorsementMinWagescaleByCardType(WagescaleList, ValidEndorsement, cardTypeId);
                    break;
                case CardTypeEnum.R:
                    result = await GetBiggerThanValidEndorsementMinWagescaleByCardType(WagescaleList, ValidEndorsement, cardTypeId);
                    break;
                case CardTypeEnum.W:
                    result = await GetBiggerThanValidEndorsementMinWagescaleByCardType(WagescaleList, ValidEndorsement, cardTypeDefinitionId);
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
