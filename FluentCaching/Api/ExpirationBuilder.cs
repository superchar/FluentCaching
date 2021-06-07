using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class ExpirationBuilder
    {
        private CachingOptions _currentOptions;

        public ExpirationBuilder(CachingOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CachingOptions CachingOptions => _currentOptions;

        public ExpirationBuilder AbsoluteExpiration() => ExpirationType(Parameters.ExpirationType.Absolute);

        public ExpirationBuilder SlidingExpiration() => ExpirationType(Parameters.ExpirationType.Sliding);

        private ExpirationBuilder ExpirationType(ExpirationType expirationType)
        {
            _currentOptions.ExpirationType = expirationType;
            return this;
        }

        public CacheImplementationBuilder And() => new CacheImplementationBuilder(_currentOptions);
    }
}
