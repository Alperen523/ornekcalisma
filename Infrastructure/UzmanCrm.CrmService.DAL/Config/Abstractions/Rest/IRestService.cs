using RestSharp;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.DAL.Config.Abstractions.Rest
{
    public interface IRestService : IApplicationService
    {
        public Task<Response<TRes>> SendRequest<TRes>(object obj, string url, string token = null,
            Method method = Method.Post, string AuthorizationType = "Bearer") where TRes : new();
    }
}
