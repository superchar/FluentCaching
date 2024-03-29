﻿using System;
using FluentCaching.Cache.Models;

namespace FluentCaching.Configuration.PolicyBuilders.Ttl;

public class TtlTypePolicyBuilder
{
    private readonly CacheOptions _currentOptions;

    internal TtlTypePolicyBuilder(CacheOptions cacheOptions)
    {
        _currentOptions = cacheOptions;
    }

    public TimeTtlPolicyBuilder SetExpirationTimeoutTo(ushort value) => new (_currentOptions, value);

    // ReSharper disable once UnusedMember.Global
    public AndPolicyBuilder<CacheImplementationPolicyBuilder> SetInfiniteExpirationTimeout()
    {
        _currentOptions.Ttl = TimeSpan.MaxValue;
        return new AndPolicyBuilder<CacheImplementationPolicyBuilder>(new CacheImplementationPolicyBuilder(_currentOptions));
    }
}