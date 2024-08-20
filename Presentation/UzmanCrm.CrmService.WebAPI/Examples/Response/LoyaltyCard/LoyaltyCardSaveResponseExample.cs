using Swashbuckle.Examples;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.LoyaltyCard
{
    public class LoyaltyCardSaveResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<LoyaltyCardSaveResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new LoyaltyCardSaveResponse
            {
                Id = System.Guid.NewGuid(),
                CardNo = "8657-6557-6765-7657",
                CrmId = System.Guid.NewGuid()
            };

            return resp;
        }
    }
}