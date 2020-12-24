
using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class CacheImplementationBuilder 
    {
        private CachingOptions _currentOptions;

        public CacheImplementationBuilder(CachingOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CachingOptions CachingOptions => _currentOptions;

        public CacheImplementationBuilder WithCacheImplementation(ICacheImplementation cacheImplementation)
        {
            _currentOptions.CacheImplementation = cacheImplementation;

            return this;
        }
    }
}
