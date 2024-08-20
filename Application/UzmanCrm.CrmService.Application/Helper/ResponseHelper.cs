using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Helper
{
    public static class ResponseHelper
    {
        public static Response<T> SetSingleError<T>(ErrorModel error) where T : new()
        {
            var model = new Response<T>();
            model.Error = error;
            model.Success = false;

            return model;
        }
    }
}
