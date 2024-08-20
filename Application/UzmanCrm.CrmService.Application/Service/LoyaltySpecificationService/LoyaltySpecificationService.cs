using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltySpecificationService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltySpecificationService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;

namespace UzmanCrm.CrmService.Application.Service.LoyaltySpecificationService
{
    public class LoyaltySpecificationService : ILoyaltySpecificationService
    {
        private readonly IDapperService dapperService;

        public LoyaltySpecificationService(IDapperService dapperService)
        {
            this.dapperService = dapperService;
        }

        /// <summary>
        /// Şu an itibariyle şimdiden küçük en güncel Sadakat Kart Tanımı kaydını çeker
        /// </summary>
        /// <returns></returns>
        public async Task<Response<ValidLoyaltySpecificationItemDto>> ValidLoyaltySpecificationGetItem()
        {
            var query = @$"
SELECT TOP 1 [uzm_loyaltyspecificationId], [uzm_discountfixingflag], [uzm_wagescalestartcalculationdate], [uzm_name]
FROM [KahveDunyasi_MSCRM].[dbo].[uzm_loyaltyspecification] WITH(NOLOCK)
WHERE statecode=0 AND [uzm_wagescalestartcalculationdate] < GETDATE()
ORDER BY [uzm_wagescalestartcalculationdate] DESC
";
            return await dapperService.GetItemParam<object, ValidLoyaltySpecificationItemDto>(query, null, GeneralHelper.GetOvmConnectionStringByCompany(Common.Enums.CompanyEnum.KD));
        }
    }
}
