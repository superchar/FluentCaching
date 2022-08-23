using FluentCaching.PolicyBuilders;

namespace FluentCaching.Memory
{
    public static class CacheImplementationBuilderExtensions
    {
        public static CacheImplementationPolicyBuilder UseInMemoryCache(
            this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder) =>
            cacheImplementationPolicyBuilder.WithCacheImplementation(new MemoryCacheImplementation());
    }
}
