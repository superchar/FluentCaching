using FluentCaching.Configuration.PolicyBuilders;

namespace FluentCaching.Memory
{
    public static class CacheImplementationBuilderExtensions
    {
        public static CacheImplementationPolicyBuilder StoreInMemory(
            this CacheImplementationPolicyBuilder cacheImplementationPolicyBuilder) =>
            cacheImplementationPolicyBuilder.StoreIn(new MemoryCacheImplementation());
    }
}
