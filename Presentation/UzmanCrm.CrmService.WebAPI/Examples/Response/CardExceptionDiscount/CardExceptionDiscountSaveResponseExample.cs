using Swashbuckle.Examples;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.CardExceptionDiscount;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.CardExceptionDiscount
{
    public class CardExceptionDiscountSaveResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<CardExceptionDiscountSaveResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new CardExceptionDiscountSaveResponse
            {
                Id = System.Guid.NewGuid(),
            };

            return resp;
        }
    }
}