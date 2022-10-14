using FluentCaching.Cache.Models;

namespace FluentCaching.Configuration.PolicyBuilders.Ttl
{
    public class ExpirationTypePolicyBuilder
    {
        private readonly CacheOptions _cacheOptions;

        public ExpirationTypePolicyBuilder(CacheOptions currentOptions)
        {
            _cacheOptions = currentOptions;
        }

        public CacheOptions CachingOptions => _cacheOptions;

        public AndPolicyBuilder<CacheImplementationPolicyBuilder> AbsoluteExpiration() 
            => ExpirationType(Cache.Models.ExpirationType.Absolute);

        public AndPolicyBuilder<CacheImplementationPolicyBuilder> SlidingExpiration() 
            => ExpirationType(Cache.Models.ExpirationType.Sliding);

        private AndPolicyBuilder<CacheImplementationPolicyBuilder> ExpirationType(ExpirationType expirationType)
        {
            _cacheOptions.ExpirationType = expirationType;
            return new AndPolicyBuilder<CacheImplementationPolicyBuilder>(new CacheImplementationPolicyBuilder(_cacheOptions));
        }
    }
}
