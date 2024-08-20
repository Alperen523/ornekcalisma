using System.Collections.Generic;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.RedisService
{
    public interface IRedisService : IApplicationService
    {
        public string SetItem<T>(T entity, string keyPrefix = "Id", double slidingExpirationHour = 24, double absoluteExpirationRelativeToNowHour = 7 * 24, bool isProp = true);
        public T GetItem<T>(string keyValue);
        public string SetList<T>(List<T> entityList, string keyValue, double slidingExpirationHour = 24, double absoluteExpirationRelativeToNowHour = 7 * 24);
        public List<T> GetList<T>(string keyValue);
        public void RemoveByPrefix<T>(string keyPrefix = "Id");
        public void RemoveByKey(string cacheKey);
    }
}
