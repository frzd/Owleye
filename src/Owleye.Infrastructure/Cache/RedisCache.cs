using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Owleye.Infrastructure.Extensions;
using Owleye.Shared.Cache;

namespace Owleye.Infrastructure.Cache
{
    public class RedisCache : IRedisCache
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCache(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task SetAsync<T>(string key, T objectToCache)
        {
            await _distributedCache.SetAsync(key, objectToCache.ObjectToByteArray());
        }

        public async Task Remove(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var obj = await _distributedCache.GetAsync(key);
            return obj == null ? default(T) : obj.ByteArrayToObject<T>();
        }
    }
}
