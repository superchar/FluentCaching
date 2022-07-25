using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using System;

namespace FluentCaching.PolicyBuilders
{
    public class CacheImplementationPolicyBuilder
    {
        private readonly CacheOptions _currentOptions;

        public CacheImplementationPolicyBuilder(CacheOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CacheOptions CachingOptions => _currentOptions;

        public CacheImplementationPolicyBuilder WithCacheImplementation(ICacheImplementation cacheImplementation)
        {
            _currentOptions.CacheImplementation = cacheImplementation
                ?? throw new ArgumentNullException(nameof(cacheImplementation), "Cache implementation cannot be null");
            return this;
        }
    }
}
