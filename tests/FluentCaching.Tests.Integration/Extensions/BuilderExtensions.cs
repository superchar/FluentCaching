using FluentCaching.Cache;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;

namespace FluentCaching.Tests.Integration.Extensions
{
    public static class BuilderExtensions
    {
        public static AndBuilder<CacheImplementationBuilder> Complete<T>(this CombinedCachingKeyBuilder<T> builder)
            where T : class
        {
            return builder.And().WithTtlOf(5).Seconds.And().SlidingExpiration();
        }

        public static CacheImplementationBuilder Complete<T>(this CombinedCachingKeyBuilder<T> builder, ICacheImplementation cacheImplementation)
            where T : class
        {
            return Complete<T>(builder)
                .And()
                .WithCacheImplementation(cacheImplementation);
        }
    }
}
