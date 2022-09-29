using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace FluentCaching.DistributedCache
{
    public class DistributedCacheImplementation : ICacheImplementation
    {
        private readonly IDistributedCache _distributedCache;

        public DistributedCacheImplementation()
        {
        }
        
        public DistributedCacheImplementation(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> RetrieveAsync<T>(string key)
        {
            using var holder = GetDistributedCacheHolder();
            var resultBytes = await holder.DistributedCache.GetAsync(key);

            return resultBytes  == null 
                ? default : JsonSerializer.Deserialize<T>(resultBytes);
        }

        public async Task CacheAsync<T>(string key, T targetObject, CacheOptions options)
        {
            using var holder = GetDistributedCacheHolder();
            var resultBytes = JsonSerializer.SerializeToUtf8Bytes(targetObject);
            await holder.DistributedCache.SetAsync(key, resultBytes, GetDistributedCacheEntryOptions(options));
        }

        public Task RemoveAsync(string key)
        {
            using var holder = GetDistributedCacheHolder();
            return holder.DistributedCache.RemoveAsync(key);
        }

        private static DistributedCacheEntryOptions GetDistributedCacheEntryOptions(CacheOptions cacheOptions)
        {
            var options = new DistributedCacheEntryOptions();
            if (cacheOptions.Ttl == TimeSpan.MaxValue)
            {
                return options;
            }
            if (cacheOptions.ExpirationType == ExpirationType.Absolute)
            {
                options.AbsoluteExpirationRelativeToNow = cacheOptions.Ttl;
            }
            else
            {
                options.SlidingExpiration = cacheOptions.Ttl;
            }

            return options;
        }

        private DistributedCacheHolder GetDistributedCacheHolder()
            => _distributedCache == null
                ? new DistributedCacheHolder()
                : new DistributedCacheHolder(_distributedCache);
    }
}

