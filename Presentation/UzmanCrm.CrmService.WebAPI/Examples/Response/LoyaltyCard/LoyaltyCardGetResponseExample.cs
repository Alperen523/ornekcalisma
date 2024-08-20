using Swashbuckle.Examples;
using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard;

namespace UzmanCrm.CrmService.WebAPI.Examples.Response.LoyaltyCard
{
    public class LoyaltyCardGetResponseExample : IExamplesProvider
    {
        public object GetExamples()
        {
            var resp = new Response<LoyaltyCardGetResponse>();
            resp.Success = true;
            resp.Message = Common.CommonStaticConsts.Message.Success;
            resp.Data = new LoyaltyCardGetResponse
            {
                CardNumber = "8657-6557-6765-7657",
                CardStatus = LoyaltyCardStatusCodeType.InUse,
                CardTypeDefinitionName = "VR",
                CustomerName = "Ahmet Demir",
                ErpId = "12345",
                Id = System.Guid.NewGuid(),
                PeriodEndorsement = 8700,
                State = "A",
                TurnoverEndorsement = 5000,
                UpdateDate = DateTime.Now,
                StoreName = "Vakko İzmir",
                CreateDate = DateTime.Now,
                ValidDiscountRate = 0
            };

            return resp;
        }
    }
}