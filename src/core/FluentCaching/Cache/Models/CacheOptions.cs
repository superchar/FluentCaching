﻿using System;
using FluentCaching.Configuration;
using FluentCaching.Keys.Builders;

namespace FluentCaching.Cache.Models;

public class CacheOptions
{
    internal CacheOptions(IKeyBuilder keyBuilder)
        => KeyBuilder = keyBuilder;
    
    public TimeSpan Ttl { get; set; }

    public ExpirationType ExpirationType { get; set; }

    internal IKeyBuilder KeyBuilder { get; }

    internal ICacheImplementation? CacheImplementation { get; set; }

    internal string PolicyName { get; set; } = CacheConfiguration.DefaultPolicyName;
}