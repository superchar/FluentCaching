using System;
using FluentCaching.Cache.Strategies.Factories;
using FluentCaching.Configuration;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Configuration.PolicyBuilders.Keys;
using FluentCaching.Keys.Builders.Factories;

namespace FluentCaching.Cache.Builders
{
    public class CacheBuilder : ICacheBuilder
    {
        private readonly ICacheConfiguration _cacheConfiguration;

        public CacheBuilder() : this(new CacheConfiguration(new KeyBuilderFactory()))
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

        public ICacheBuilder For<T>(
            Func<CachingKeyPolicyBuilder<T>, AndPolicyBuilder<CacheImplementationPolicyBuilder>> factoryFunc)
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

        public ICache Build() => new Cache(new CacheStrategyFactory(_cacheConfiguration));
    }
}