using Swashbuckle.Examples;
using System;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.Contact
{
    public class SaveCustomerRequestExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new SaveCustomerRequest
            {
                BirthDate = Convert.ToDateTime("1995-01-15"),
                EmailAddress = "info@example.com",
                EmailPermit = true,
                EmailPermitDate = DateTime.Now,
                FirstName = "DENEME",
                LastName = "TEST",
                MobilePhone = "5065811342",
                CallPermit = false,
                CallPermitDate = DateTime.Now,
                SmsPermit = true,
                SmsPermitDate = DateTime.Now,
                GenderId = Common.Enums.GenderEnum.Erkek,
                ChannelId = Common.Enums.ChannelEnum.ETicaret,
                PersonNo = "2890",
                CrmId = Guid.Empty,
                CustomerTypeId = Common.Enums.CustomerTypeEnum.GercekMusteri,
                KvkkPermit = true,
                MobileId = Guid.NewGuid().ToString(),
                MobileAppDownloadedDate = DateTime.Now,
                StoreCode = "STR01"

            };

        }
    }
}