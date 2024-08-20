using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.CustomerGroup;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.CustomerGroup
{
    public class CustomerGroupGetResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<List<CustomerGroupGetResponse>>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new List<CustomerGroupGetResponse>
            {
                new CustomerGroupGetResponse
                {
                    Id = System.Guid.NewGuid(),
                    Code = "3",
                    CustomerGroupName = "İş Dünyası"
                },
            };

            return resp;
        }
    }
}