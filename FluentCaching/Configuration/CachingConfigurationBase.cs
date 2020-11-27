using System;
using FluentCaching.Api;
using FluentCaching.Api.Keys;

namespace FluentCaching.Configuration
{
    public abstract class CachingConfigurationBase
    {
        internal abstract ICacheImplementation Current { get; }

        internal abstract CachingConfigurationItem<T> GetItem<T>()
            where T : class;
    }
}