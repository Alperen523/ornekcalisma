using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Loyalty
{
    public class SaveLoyaltyCardResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DataModel Data { get; set; }
        public ErrorModelCard Error { get; set; }
    }
    public class DataModel
    {
        public string Id { get; set; }
        public string CrmId { get; set; }
        public string CardNo { get; set; }
    }   
    public class ErrorModelCard
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
        public string Description { get; set; }
        public string ErrorCode { get; set; }
    }
}
