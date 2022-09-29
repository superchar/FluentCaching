using FluentCaching.PolicyBuilders;
using Microsoft.Extensions.Caching.Distributed;

namespace FluentCaching.DistributedCache;

public static class CacheImplementationBuilderExtensions
{
    public static CacheImplementationPolicyBuilder UseDistributedCache(
        this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder) =>
        cacheImplementationPolicyBuilder.WithCacheImplementation(new DistributedCacheImplementation());
    
    public static CacheImplementationPolicyBuilder UseDistributedCache(
        this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder, IDistributedCache distributedCache) =>
        cacheImplementationPolicyBuilder.WithCacheImplementation(new DistributedCacheImplementation(distributedCache));
}