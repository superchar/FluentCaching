using FluentCaching.Cache.Models;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class ExpirationTypeBuilder
    {
        private readonly CacheOptions _cacheOptions;

        public ExpirationTypeBuilder(CacheOptions currentOptions)
        {
            _cacheOptions = currentOptions;
        }

        public CacheOptions CachingOptions => _cacheOptions;

        public AndBuilder<CacheImplementationBuilder> AbsoluteExpiration() 
            => ExpirationType(Cache.Models.ExpirationType.Absolute);

        public AndBuilder<CacheImplementationBuilder> SlidingExpiration() 
            => ExpirationType(Cache.Models.ExpirationType.Sliding);

        private AndBuilder<CacheImplementationBuilder> ExpirationType(ExpirationType expirationType)
        {
            _cacheOptions.ExpirationType = expirationType;
            return new AndBuilder<CacheImplementationBuilder>(new CacheImplementationBuilder(_cacheOptions));
        }
    }
}
