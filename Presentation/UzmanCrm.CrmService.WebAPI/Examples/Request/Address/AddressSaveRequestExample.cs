using Swashbuckle.Examples;
using UzmanCrm.CrmService.WebAPI.Models.Address;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.Address
{

    public class AddressSaveRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new AddressSaveRequest
            {
                AddressId = "1234567",
                AddressType = Common.Enums.AddressTypeEnum.Fatura,
                ChannelId = Common.Enums.ChannelEnum.ETicaret,
                Location = "2890",
                CountryCode = Common.Enums.CountryCodeEnum.TR,
                CityCode = "90216",
                DistrictCode = "612",
                NeighborhoodCode = "64493",
                AddressLine = "Açık Adres bilgisi",
                ErpId = "12345",
                IsDefaultAddress = true,
                PostCode = "34930"


            };
        }
    }
}