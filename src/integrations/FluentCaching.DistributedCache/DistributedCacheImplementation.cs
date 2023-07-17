using System;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace FluentCaching.DistributedCache;

public class DistributedCacheImplementation : ICacheImplementation
{
    private readonly IDistributedCache? _distributedCache;
    private readonly IDistributedCacheSerializer[]? _cacheSerializers;

    public DistributedCacheImplementation()
    {
    }
        
    public DistributedCacheImplementation(IDistributedCache distributedCache, 
        IDistributedCacheSerializer[]? cacheSerializers)
    {
        _distributedCache = distributedCache;
        _cacheSerializers = cacheSerializers;
    }

    public async ValueTask<TEntity?> RetrieveAsync<TEntity>(string key)
    {
        using var cacheHolder = GetDistributedCacheHolder();
        using var serializerHolder = GetCacheSerializerHolder<TEntity>();
        var resultBytes = await cacheHolder.Cache.GetAsync(key);

        return resultBytes == null || resultBytes.Length == 0
            ? default
            : await serializerHolder.Serializer.DeserializeAsync<TEntity>(resultBytes);
    }

    public async ValueTask CacheAsync<TEntity>(string key, TEntity entity, CacheOptions options)
        where TEntity : notnull
    {
        using var cacheHolder = GetDistributedCacheHolder();
        using var serializerHolder = GetCacheSerializerHolder<TEntity>();
        var resultBytes = await serializerHolder.Serializer.SerializeAsync(entity);
        
        await cacheHolder.Cache.SetAsync(key, resultBytes, GetDistributedCacheEntryOptions(options));
    }

    public async ValueTask RemoveAsync(string key)
    {
        using var holder = GetDistributedCacheHolder();
        await holder.Cache.RemoveAsync(key);
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

    private DistributedCacheSerializerHolder<TEntity> GetCacheSerializerHolder<TEntity>()
        => _cacheSerializers == null
            ? new DistributedCacheSerializerHolder<TEntity>()
            : new DistributedCacheSerializerHolder<TEntity>(_cacheSerializers);
}