using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using FluentCaching.Parameters;

namespace FluentCaching.Memory
{
    public class MemoryCacheImplementation : ICacheImplementation
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public Task<T> GetAsync<T>(string key)
        {
            return Cache.Contains(key) ? Task.FromResult((T)Cache[key]) : Task.FromResult(default(T));
        }

        public Task SetAsync<T>(string key, T targetObject, CachingOptions options)
        {
            Cache.Set(key, targetObject, CreatePolicy(options));
            return Task.CompletedTask;
        }

        private static CacheItemPolicy CreatePolicy(CachingOptions options) =>
            options.ExpirationType == ExpirationType.Sliding ?
                new CacheItemPolicy
                {
                    SlidingExpiration = options.Ttl
                } :
                new CacheItemPolicy
                {
                    AbsoluteExpiration = options.Ttl != TimeSpan.MaxValue ? DateTimeOffset.UtcNow.Add(options.Ttl) : ObjectCache.InfiniteAbsoluteExpiration
                };

        public Task RemoveAsync(string key)
        {
            Cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}
