using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.Memory
{
    public class MemoryCacheImplementation : ICacheImplementation
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public ValueTask<T> RetrieveAsync<T>(string key)
        {
            return Cache.Contains(key) ? new ValueTask<T>((T)Cache[key]) : new ValueTask<T>(default(T));
        }

        public ValueTask CacheAsync<T>(string key, T targetObject, CacheOptions options)
        {
            Cache.Set(key, targetObject, CreatePolicy(options));
            return default;
        }

        private static CacheItemPolicy CreatePolicy(CacheOptions options) =>
            options.ExpirationType == ExpirationType.Sliding ?
                new CacheItemPolicy
                {
                    SlidingExpiration = options.Ttl
                } :
                new CacheItemPolicy
                {
                    AbsoluteExpiration = options.Ttl != TimeSpan.MaxValue ? DateTimeOffset.UtcNow.Add(options.Ttl) : ObjectCache.InfiniteAbsoluteExpiration
                };

        public ValueTask RemoveAsync(string key)
        {
            Cache.Remove(key);
            return default;
        }
    }
}
