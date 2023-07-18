using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
using FluentCaching.Configuration.PolicyBuilders.Ttl;

namespace FluentCaching.Tests.Integration.Extensions;

public static class BuilderExtensions
{
    public static AndPolicyBuilder<CacheImplementationPolicyBuilder> Complete<T>(this CombinedCachingKeyPolicyBuilder<T> policyBuilder)
        where T : class
    {
        return policyBuilder.And().SetExpirationTimeoutTo(5).Seconds.With().SlidingExpiration();
    }
    
    public static AndPolicyBuilder<CacheImplementationPolicyBuilder> Complete(this AndPolicyBuilder<TtlTypePolicyBuilder> policyBuilder)
    {
        return policyBuilder.And().SetExpirationTimeoutTo(5).Seconds.With().SlidingExpiration();
    }
}