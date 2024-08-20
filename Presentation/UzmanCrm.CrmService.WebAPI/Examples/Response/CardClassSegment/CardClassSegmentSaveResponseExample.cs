using Swashbuckle.Examples;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.CardClassSegment;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.CardClassSegment
{
    public class CardClassSegmentSaveResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<CardClassSegmentSaveResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new CardClassSegmentSaveResponse
            {
                Id = System.Guid.NewGuid(),
            };

            return resp;
        }
    }
}