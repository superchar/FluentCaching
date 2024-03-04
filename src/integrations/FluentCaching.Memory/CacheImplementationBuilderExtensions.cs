using FluentCaching.Cache.Builders;
using FluentCaching.Configuration.PolicyBuilders;

namespace FluentCaching.Memory;

// ReSharper disable once UnusedType.Global
public static class CacheImplementationBuilderExtensions
{
    // ReSharper disable once UnusedMember.Global
    public static CacheImplementationPolicyBuilder StoreInMemory(
        this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder) =>
        cacheImplementationPolicyBuilder.StoreIn(new MemoryCacheImplementation());

    public static ICacheBuilder SetInMemoryAsDefaultCache(this ICacheBuilder cacheBuilder)
        => cacheBuilder.SetGenericCache(new MemoryCacheImplementation());
}