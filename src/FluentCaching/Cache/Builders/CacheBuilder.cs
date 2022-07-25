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
        private readonly ICacheConfiguration _cacheConfiguration;

        public CacheBuilder() : this(new CacheConfiguration())
        {
        }

        internal CacheBuilder(ICacheConfiguration cacheConfiguration)
        {
            _cacheConfiguration = cacheConfiguration;
        }

        public ICacheBuilder For<T>(Func<CachingKeyPolicyBuilder<T>, CacheImplementationPolicyBuilder> factoryFunc)
            where T : class
        {
            _cacheConfiguration.For(factoryFunc);
            return this;
        }

        public ICacheBuilder For<T>(Func<CachingKeyPolicyBuilder<T>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc)
            where T : class
        {
            _cacheConfiguration.For(factoryFunc);
            return this;
        }

        public ICacheBuilder SetGenericCache(ICacheImplementation cacheImplementation)
        {
            _cacheConfiguration.SetGenericCache(cacheImplementation);
            return this;
        }

        public ICache Build() => new Cache(new CacheService(_cacheConfiguration, new ConcurrencyHelper()));
    }
}
