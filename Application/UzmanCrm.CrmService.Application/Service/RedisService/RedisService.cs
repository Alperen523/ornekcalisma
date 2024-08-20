using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UzmanCrm.CrmService.Application.Abstractions.Service.RedisService;

namespace UzmanCrm.CrmService.Application.Service.RedisService
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache redisCache;

        public RedisService(IDistributedCache cache)
        {
            redisCache = cache;
        }

        public string SetItem<T>(T entity, string keyPrefix = "Id", double slidingExpirationHour = 24, double absoluteExpirationRelativeToNowHour = 7 * 24, bool isProp = true)
        {
            try
            {
                var key = string.Empty;
                var entityType = entity.GetType();
                if (isProp)
                {
                    var property = entity.GetType().GetProperty(keyPrefix).GetValue(entity);

                    if (property == null)
                    {
                        return default;
                    }
                    key = entityType.Name + "_" + property;
                }
                else
                {
                    key = entityType.Name + "_" + keyPrefix;
                }


                var timeOut = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(absoluteExpirationRelativeToNowHour),
                    SlidingExpiration = TimeSpan.FromHours(slidingExpirationHour)
                };

                redisCache.SetString(key, JsonConvert.SerializeObject(entity, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }), timeOut);

                return key;
            }
            catch (Exception e)
            {
                return default;
            }

        }

        public T GetItem<T>(string keyValue)
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue)) return default;

                var key = typeof(T).Name + "_" + keyValue;

                var value = redisCache.GetString(key);

                if (!string.IsNullOrEmpty(value))
                {
                    return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }

                return default;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public string SetList<T>(List<T> entityList, string keyValue, double slidingExpirationHour = 24, double absoluteExpirationRelativeToNowHour = 7 * 24)
        {
            try
            {
                var entityType = entityList.FirstOrDefault().GetType();

                var key = "List_" + entityType.Name + "_" + keyValue;

                var timeOut = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(absoluteExpirationRelativeToNowHour),
                    SlidingExpiration = TimeSpan.FromHours(slidingExpirationHour)
                };

                redisCache.SetString(key, JsonConvert.SerializeObject(entityList), timeOut);

                return key;
            }
            catch (Exception e)
            {
                return default;
            }

        }

        public List<T> GetList<T>(string keyValue)
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue)) return default;

                var key = "List_" + typeof(T).Name + "_" + keyValue;

                var value = redisCache.GetString(key);

                if (!string.IsNullOrEmpty(value))
                {
                    return JsonConvert.DeserializeObject<List<T>>(value);
                }

                return default;
            }
            catch (Exception e)
            {
                return default;
            }

        }

        public void RemoveByPrefix<T>(string keyPrefix = "Id")
        {
            try
            {
                var key = typeof(T).Name + "_" + keyPrefix;

                redisCache.Remove(key);
            }
            catch (Exception e)
            {
                return;
            }

        }

        public void RemoveByKey(string cacheKey)
        {
            try
            {
                redisCache.Remove(cacheKey);
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
