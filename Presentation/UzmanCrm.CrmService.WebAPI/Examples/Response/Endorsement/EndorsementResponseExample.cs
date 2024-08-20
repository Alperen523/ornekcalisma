using Swashbuckle.Examples;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.Endorsement;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.Endorsement
{
    public class EndorsementResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<EndorsementSaveResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new EndorsementSaveResponse
            {
                Id = System.Guid.NewGuid()
            };

            return resp;
        }
    }
}