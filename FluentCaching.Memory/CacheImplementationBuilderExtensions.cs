using FluentCaching.PolicyBuilders;

namespace FluentCaching.Memory
{
    public static class CacheImplementationBuilderExtensions
    {
        public static CacheImplementationBuilder WithInMemoryCache(
            this CacheImplementationBuilder cacheImplementationBuilder) =>
            cacheImplementationBuilder.WithCacheImplementation(new MemoryCacheImplementation());
    }
}
