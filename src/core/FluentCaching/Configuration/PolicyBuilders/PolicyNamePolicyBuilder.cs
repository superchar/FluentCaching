using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders.Ttl;

namespace FluentCaching.Configuration.PolicyBuilders;

public class PolicyNamePolicyBuilder
{
    private readonly CacheOptions _currentCacheOptions;
    
    public PolicyNamePolicyBuilder(string name, CacheOptions cacheOptions)
    {
        _currentCacheOptions = cacheOptions;
        _currentCacheOptions.PolicyName = name;
    }

    public AndPolicyBuilder<TtlTypePolicyBuilder> PolicyName()
        => new (new TtlTypePolicyBuilder(_currentCacheOptions));
}
