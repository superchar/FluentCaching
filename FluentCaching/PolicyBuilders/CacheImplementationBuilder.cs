using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.PolicyBuilders
{
    public class CacheImplementationBuilder
    {
        private CacheOptions _currentOptions;

        public CacheImplementationBuilder(CacheOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CacheOptions CachingOptions => _currentOptions;

        public CacheImplementationBuilder WithCacheImplementation(ICacheImplementation cacheImplementation)
        {
            _currentOptions.CacheImplementation = cacheImplementation;
            return this;
        }
    }
}
