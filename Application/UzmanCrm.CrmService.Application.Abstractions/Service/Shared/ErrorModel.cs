using System.Net;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.Shared
{
    public class ErrorModel
    {
        public ErrorModel(HttpStatusCode StatucCode, string Description, string ErrorCode)
        {
            this.StatusCode = StatucCode;
            this.Description = Description;
            this.ErrorCode = ErrorCode;
        }
        public ErrorModel()
        {
           
        }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;
        public string Description { get; set; }
        public string ErrorCode { get; set; }
    }
}
