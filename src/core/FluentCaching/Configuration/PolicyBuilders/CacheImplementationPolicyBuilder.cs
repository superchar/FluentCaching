using System;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.Configuration.PolicyBuilders
{
    public class CacheImplementationPolicyBuilder
    {
        public CacheImplementationPolicyBuilder(CacheOptions currentOptions)
        {
            CachingOptions = currentOptions;
        }

        public CacheOptions CachingOptions { get; }

        public CacheImplementationPolicyBuilder StoreIn(ICacheImplementation cacheImplementation)
        {
            CachingOptions.CacheImplementation = cacheImplementation
                ?? throw new ArgumentNullException(nameof(cacheImplementation), "Cache implementation cannot be null");
            return this;
        }
    }
}
