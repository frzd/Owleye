using System;
using System.Threading.Tasks;
using EasyCaching.Core;
using Microsoft.Extensions.Configuration;
using Owleye.Shared.Cache;

namespace Owleye.Infrastructure.Cache
{
    public class RedisCache : IRedisCache
    {
        private readonly IEasyCachingProviderFactory _distributedCache;
        private readonly IConfiguration _configuration;

        public RedisCache(
            IEasyCachingProviderFactory distributedCache,
            IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        public async Task SetAsync<T>(string key, T objectToCache)
        {
            // TODO refactor this.
            var provider = _distributedCache.GetCachingProvider(_configuration["General:RedisInstanceName"]);

            await provider.SetAsync(key, objectToCache, TimeSpan.FromDays(90));

        }

        public async Task Remove(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string key)
        {

            // TODO refactor this.
            var provider = _distributedCache.GetCachingProvider(_configuration["General:RedisInstanceName"]);

            var cachedResult = await provider.GetAsync<T>(key);
            return cachedResult.Value;

        }
    }
}
