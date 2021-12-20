using FluentCaching.Cache.Helpers;
using FluentCaching.Configuration;
using FluentCaching.PolicyBuilders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.PolicyBuilders.Ttl;
using System;

namespace FluentCaching.Cache.Builders
{
    public class CacheBuilder : ICacheBuilder
    {
        private readonly CacheConfiguration _cacheConfiguration = new CacheConfiguration();

        public CacheBuilder For<T>(Func<CachingKeyBuilder<T>, CacheImplementationBuilder> factoryFunc)
            where T : class
        {
            _cacheConfiguration.For(factoryFunc);
            return this;
        }

        public CacheBuilder For<T>(Func<CachingKeyBuilder<T>, ExpirationTypeBuilder> factoryFunc)
            where T : class
        {
            _cacheConfiguration.For(factoryFunc);
            return this;
        }

        public CacheBuilder SetGenericCache(ICacheImplementation cacheImplementation)
        {
            _cacheConfiguration.SetImplementation(cacheImplementation);
            return this;
        }

        public ICache Build() => new Cache(new StoringService(_cacheConfiguration, new ConcurrencyHelper()));
    }
}
