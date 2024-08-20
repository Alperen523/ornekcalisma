using UzmanCrm.CrmService.Common;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.Shared
{
    public class ResponseError<T> where T : new()
    {

        public bool Success { get; set; } = false;

        public string Message { get; set; } = CommonStaticConsts.Message.Unsuccess;

        public ErrorModel Error { get; set; }
    }

}
