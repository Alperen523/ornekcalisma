using Swashbuckle.Examples;
using UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.LoyaltyCard
{

    public class LoyaltyCardRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new LoyaltyCardRequest
            {
                ErpId = "12345",
                CardType = Common.Enums.CardTypeEnum.V,
                StoreCode = "1122"
            };
        }
    }
}