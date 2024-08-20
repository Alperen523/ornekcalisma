using Swashbuckle.Examples;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.Contact
{
    public class GetCustomerRequestExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetCustomerRequest
            {
                MobilePhone = "5065811342",

            };

        }
    }
}