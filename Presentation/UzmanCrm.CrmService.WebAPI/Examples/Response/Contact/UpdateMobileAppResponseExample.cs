﻿using Swashbuckle.Examples;
using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.Contact
{
    public class UpdateMobileAppResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<UpdateMobileAppResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new UpdateMobileAppResponse
            {

                CrmId = new Guid("574b0e7a-b7c0-4217-aace-3ff974677607"),
                Type = Common.Enums.CreateType.Create

            };

            return resp;

        }

    }
}