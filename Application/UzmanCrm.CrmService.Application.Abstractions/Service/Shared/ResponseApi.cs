using UzmanCrm.CrmService.Common;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.Shared
{


    public class ResponseApi<T> where T : new()
    {
        public ResponseApi()
        {
            this.Data = new T();
        }

        public string Message { get; set; } = CommonStaticConsts.Message.Unsuccess;

        public T Data { get; set; }

        public ErrorModel Error { get; set; }
    }
}
