using Swashbuckle.Examples;
using UzmanCrm.CrmService.WebAPI.Models.Endorsement;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.Endorsement
{
    public class EndorsementRequestExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new EndorsementRequest
            {
                BillType = Common.Enums.BillTypeEnum.ZP04,
                ErpId = "12345",
                GiftCardAmount = 0,
                InvoiceNumber = "INV12345",
                OrderDate = System.DateTime.Parse("2022-09-11"),
                OrderNumber = "ORDR12345",
                InvoiceDate = System.DateTime.Parse("2022-09-11"),
                StoreCode = "2890",
                TotalAmount = (decimal)103.34,
                TransactionId = "SAP1000",
                CardNo = "123456789000"
            };
        }
    }
}