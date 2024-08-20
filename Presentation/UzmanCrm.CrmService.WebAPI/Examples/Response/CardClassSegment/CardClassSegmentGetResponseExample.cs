using Swashbuckle.Examples;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.WebAPI.Models.CardClassSegment;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.CardClassSegment
{
    public class CardClassSegmentGetResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<List<CardClassSegmentGetResponse>>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new List<CardClassSegmentGetResponse>
            {
                new CardClassSegmentGetResponse
                {
                    Id = System.Guid.NewGuid(),
                    SegmentName = "F10",
                    Description = "Aile",
                    ValidityPeriod = 365,
                    FirstNotificationPeriod = 90,
                    SecondNotificationPeriod = 30
                },
            };

            return resp;
        }
    }
}