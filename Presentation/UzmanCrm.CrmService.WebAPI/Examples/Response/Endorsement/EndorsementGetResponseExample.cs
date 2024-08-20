using Swashbuckle.Examples;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Models.Endorsement;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.Endorsement
{
    public class EndorsementGetResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<EndorsementGetResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new EndorsementGetResponse
            {
                AmountForUpperSegment = 1730,
                CardType = "W",
                DiscountPercent = 5,
                UpperSegmentDiscountPercent = 10,
                Endorsement = 8270,
                EndorsementType = EndorsementType.Standart,
            };

            return resp;
        }
    }
}