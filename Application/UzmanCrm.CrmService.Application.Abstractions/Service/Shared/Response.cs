using UzmanCrm.CrmService.Common;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.Shared
{
    public class Response<T> where T : new()
    {
        public Response()
        {
            this.Data = new T();
        }

        public bool Success { get; set; } = false;

        public string Message { get; set; } = CommonStaticConsts.Message.Unsuccess;

        public T Data { get; set; }

        public ErrorModel Error { get; set; }
    }

}
