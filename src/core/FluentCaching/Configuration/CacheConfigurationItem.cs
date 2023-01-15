﻿using FluentCaching.Cache.Models;

namespace FluentCaching.Configuration;

internal class CacheConfigurationItem : ICacheConfigurationItem
{
    public CacheConfigurationItem(CacheOptions options)
    {
        Options = options;
    }
        
    public CacheOptions Options { get; }
}