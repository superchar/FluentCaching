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
        this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder, IDistributedCache distributedCache) =>
        cacheImplementationPolicyBuilder.StoreIn(new DistributedCacheImplementation(distributedCache));
}