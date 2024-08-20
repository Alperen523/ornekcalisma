using Swashbuckle.Examples;
using UzmanCrm.CrmService.WebAPI.Models.Address;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.Address
{

    public class DeleteAddressRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new DeleteAddressRequest
            {
                AddressId = "1234567",
                Location = "2890",
                EcomId = "12345"
            };
        }
    }
}