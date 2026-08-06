using Microsoft.Extensions.Caching.Memory;
using PestanaDevApi.Interfaces;

namespace PestanaDevApi.Services
{
    public class CacheService: ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? Get<T>(string key)
        {
            _cache.TryGetValue(key, out T? value);

            return value;
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            _cache.Set(key, value, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(24)});
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
