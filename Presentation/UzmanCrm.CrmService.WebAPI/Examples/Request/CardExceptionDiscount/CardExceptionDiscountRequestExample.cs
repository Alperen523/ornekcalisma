using Swashbuckle.Examples;
using UzmanCrm.CrmService.WebAPI.Models.CardExceptionDiscount;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.CardExceptionDiscount
{
    public class CardExceptionDiscountRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new CardExceptionDiscountRequest
            {
                CardClassSegmentId = System.Guid.NewGuid(),
                CustomerGroupId = System.Guid.NewGuid(),
                DemandedUserId = System.Guid.NewGuid(),
                LoyaltyCardId = System.Guid.NewGuid(),
                DemandDate = System.DateTime.Now,
                StartDate = System.DateTime.Now,
                EndDate = System.DateTime.Now,
                DiscountRate = 10,
                Description = "Ahmet Demir yakın çevre kart istisna tanımlama",
                ApprovalStatus = Common.Enums.ApprovalStatusType.Draft,
                ApprovedByUserId = System.Guid.NewGuid(),
                StatusCode = Common.Enums.CardDiscountStatusCodeType.Draft
            };
        }
    }
}