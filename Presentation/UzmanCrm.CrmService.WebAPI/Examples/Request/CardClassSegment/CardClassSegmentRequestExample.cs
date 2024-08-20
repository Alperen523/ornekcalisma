using Swashbuckle.Examples;
using UzmanCrm.CrmService.WebAPI.Models.CardClassSegment;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.CardClassSegment
{
    public class CardClassSegmentRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CardClassSegmentRequest
            {
                SegmentName = "F1",
                ValidityPeriod = 365,
                FirstNotificationPeriod = 90,
                SecondNotificationPeriod = 30,
                Description = "Aile"
            };
        }
    }
}