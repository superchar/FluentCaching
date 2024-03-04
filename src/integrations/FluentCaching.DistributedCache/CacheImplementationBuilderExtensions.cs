using FluentCaching.Cache.Builders;
using FluentCaching.Configuration.PolicyBuilders;
using Microsoft.Extensions.Caching.Distributed;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace FluentCaching.DistributedCache;

public static class CacheImplementationBuilderExtensions
{
    public static CacheImplementationPolicyBuilder StoreInDistributedCache(
        this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder) =>
        cacheImplementationPolicyBuilder.StoreIn(new DistributedCacheImplementation());
    
    public static CacheImplementationPolicyBuilder StoreInDistributedCache(
        this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder, 
        IDistributedCache distributedCache, 
        params IDistributedCacheSerializer[] serializers) =>
        cacheImplementationPolicyBuilder.StoreIn(new DistributedCacheImplementation(distributedCache, serializers));
    
    public static ICacheBuilder SetDistributedAsDefaultCache(this ICacheBuilder cacheBuilder)
        => cacheBuilder.SetGenericCache(new DistributedCacheImplementation());

    public static ICacheBuilder SetDistributedAsDefaultCache(this ICacheBuilder cacheBuilder,
        IDistributedCache distributedCache,
        params IDistributedCacheSerializer[] serializers)
        => cacheBuilder.SetGenericCache(new DistributedCacheImplementation(distributedCache, serializers));
}