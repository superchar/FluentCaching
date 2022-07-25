using FluentCaching.PolicyBuilders;

namespace FluentCaching.Memory
{
    public static class CacheImplementationBuilderExtensions
    {
        public static CacheImplementationPolicyBuilder WithInMemoryCache(
            this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder) =>
            cacheImplementationPolicyBuilder.WithCacheImplementation(new MemoryCacheImplementation());
    }
}
