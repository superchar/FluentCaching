using System;
using FluentCaching.Cache.Models;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Configuration.PolicyBuilders.Ttl;

public class TtlTypePolicyBuilder
{
    private readonly CacheOptions _currentOptions;

    internal TtlTypePolicyBuilder(IKeyBuilder keyBuilder)
    {
        _currentOptions = new CacheOptions(keyBuilder);
    }

    public TimeTtlPolicyBuilder SetExpirationTimeoutTo(ushort value) => new (_currentOptions, value);

    // ReSharper disable once UnusedMember.Global
    public AndPolicyBuilder<CacheImplementationPolicyBuilder> SetInfiniteExpirationTimeout()
    {
        _currentOptions.Ttl = TimeSpan.MaxValue;
        return new AndPolicyBuilder<CacheImplementationPolicyBuilder>(new CacheImplementationPolicyBuilder(_currentOptions));
    }
}