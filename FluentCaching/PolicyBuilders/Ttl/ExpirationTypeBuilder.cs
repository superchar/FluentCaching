using FluentCaching.Cache.Models;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class ExpirationTypeBuilder
    {
        private CacheOptions _currentOptions;

        public ExpirationTypeBuilder(CacheOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CacheOptions CachingOptions => _currentOptions;

        public ExpirationTypeBuilder AbsoluteExpiration() => ExpirationType(FluentCaching.Cache.Models.ExpirationType.Absolute);

        public ExpirationTypeBuilder SlidingExpiration() => ExpirationType(FluentCaching.Cache.Models.ExpirationType.Sliding);

        private ExpirationTypeBuilder ExpirationType(ExpirationType expirationType)
        {
            _currentOptions.ExpirationType = expirationType;
            return this;
        }

        public CacheImplementationBuilder And() => new CacheImplementationBuilder(_currentOptions);
    }
}
