using FluentCaching.Cache;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;

namespace FluentCaching.Tests.Integration.Extensions
{
    public static class BuilderExtensions
    {
        public static AndPolicyBuilder<CacheImplementationPolicyBuilder> Complete<T>(this CombinedCachingKeyPolicyBuilder<T> policyBuilder)
            where T : class
        {
            return policyBuilder.And().WithTtlOf(5).Seconds.And().SlidingExpiration();
        }

        public static CacheImplementationPolicyBuilder Complete<T>(this CombinedCachingKeyPolicyBuilder<T> policyBuilder, ICacheImplementation cacheImplementation)
            where T : class
        {
            return Complete<T>(policyBuilder)
                .And()
                .WithCacheImplementation(cacheImplementation);
        }
    }
}
