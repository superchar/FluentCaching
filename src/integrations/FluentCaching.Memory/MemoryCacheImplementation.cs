using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.Memory;

public class MemoryCacheImplementation : ICacheImplementation
{
    private static readonly ObjectCache Cache = MemoryCache.Default;

    public ValueTask<TEntity?> RetrieveAsync<TEntity>(string key)
        => Cache.Contains(key)
            ? new ValueTask<TEntity?>((TEntity)Cache[key])
            : new ValueTask<TEntity?>(default(TEntity));

    public ValueTask CacheAsync<TEntity>(string key, TEntity entity, CacheOptions options)
        where TEntity : notnull
    {
        Cache.Set(key, entity, CreatePolicy(options));
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