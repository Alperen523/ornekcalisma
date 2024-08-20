using Swashbuckle.Examples;

using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.Address;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.Address
{
    public class DeleteAddressResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<DeleteAddressResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new DeleteAddressResponse
            {
                CrmId = System.Guid.NewGuid(),
                Id = System.Guid.NewGuid(),
                ErpId = "654321"

            };

            return resp;
        }
    }
}