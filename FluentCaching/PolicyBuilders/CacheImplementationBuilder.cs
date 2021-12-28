using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using System;

namespace FluentCaching.PolicyBuilders
{
    public class CacheImplementationBuilder
    {
        private readonly CacheOptions _currentOptions;

        public CacheImplementationBuilder(CacheOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CacheOptions CachingOptions => _currentOptions;

        public CacheImplementationBuilder WithCacheImplementation(ICacheImplementation cacheImplementation)
        {
            _currentOptions.CacheImplementation = cacheImplementation
                ?? throw new ArgumentNullException(nameof(cacheImplementation), "Cache implementation cannot be null");
            return this;
        }
    }
}
