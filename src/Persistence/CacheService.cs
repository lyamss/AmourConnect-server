using Microsoft.Extensions.Caching.Distributed;
using Infrastructure.Interfaces;
using System.Text.Json;

namespace Infrastructure.Persistence
{
    internal class CacheService(IDistributedCache cache) : ICacheService
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await _cache.GetStringAsync(key);
            if(json == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpirationTime = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationTime ?? TimeSpan.FromMinutes(30)
            };
            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options);
        }
    }
}