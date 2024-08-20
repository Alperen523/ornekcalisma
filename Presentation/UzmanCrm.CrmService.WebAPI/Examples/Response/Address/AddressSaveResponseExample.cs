using Swashbuckle.Examples;

using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.Address;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.Address
{
    public class AddressSaveResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<AddressSaveResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new AddressSaveResponse
            {
                CrmId = System.Guid.NewGuid(),
                Id = System.Guid.NewGuid(),
                Type = Common.Enums.CreateType.Create

            };

            return resp;
        }
    }
}