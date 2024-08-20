using Swashbuckle.Examples;
using System;
using UzmanCrm.CrmService.WebAPI.Models.Contact;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.Contact
{
    public class GetCustomerByEcomIdResponseExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GetCustomerByEcomIdResponse
            {
                BirthDate = Convert.ToDateTime("1995-01-15"),
                FirstName = "DENEME",
                LastName = "TEST",
                GenderId = Common.Enums.GenderEnum.Erkek,
                CardNo = "0035571000687699",
                CardType = "V",
                CrmId = new Guid("574b0e7a-b7c0-4217-aace-3ff974677607"),
                IsKvkk = true,
                CustomerType = "M",
                ErpId = "5019299",
                LastUpdatedDate = DateTime.Now,
                StatusEnum = Common.Enums.StatusType.Aktif,
                EmailAddress = "example@example.com",
                EmailPermit = true,
                PhoneNumber = "5559999999",
                CallPermit = false,
                SmsPermit = true


            };

        }
    }
}