using Swashbuckle.Examples;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.LoyaltyCard
{
    public class LoyaltyCardGetRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new LoyaltyCardGetRequest
            {
                CardNo = "1234567890123456",
                ErpId = "12345",
                ChannelType = CardTypeEnum.W
            };
        }
    }
}