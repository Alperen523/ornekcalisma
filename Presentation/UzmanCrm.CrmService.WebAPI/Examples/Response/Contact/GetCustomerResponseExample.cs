using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.Contact
{
    public class GetCustomerResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new ResponseApi<List<GetCustomerResponse>>();
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new List<GetCustomerResponse> { new GetCustomerResponse
                {
                    CrmId= new Guid("574b0e7a-b7c0-4217-aace-3ff974677607"),
                    CustomerTypeId=CustomerTypeEnum.GercekMusteri,
                    FirstName= "Test",
                    LastName= "Deneme",
                    GenderId= GenderEnum.Bilinmiyor,
                    MobilePhone="5055555555",
                    EmailAddress="",
                    CallPermit=true,
                    SmsPermit=true,
                    EmailPermit=false,
                    CountryId= 90,
                    CityId= 216,
                    DisctrictId= 2084,
                    NeighborhoodId= 64598,
                    AdressLine= "İstanbul / Kadıköy",
                    BirthDate= DateTime.Now,
                    LoyaltyActivationDate= DateTime.Now,


                }
            };

            return resp;

        }

    }
}