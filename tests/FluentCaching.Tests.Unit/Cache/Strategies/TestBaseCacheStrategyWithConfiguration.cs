using FluentCaching.Cache;
using FluentCaching.Cache.Strategies;
using FluentCaching.Configuration;

namespace FluentCaching.Tests.Unit.Cache.Strategies
{
    internal class TestBaseCacheStrategyWithConfiguration : BaseCacheStrategyWithConfiguration
    {
        public TestBaseCacheStrategyWithConfiguration(ICacheConfiguration configuration) : base(configuration)
        {
        }

        public new ICacheConfigurationItem<T> GetConfigurationItem<T>() where T : class 
            => base.GetConfigurationItem<T>();

        public new ICacheImplementation GetCacheImplementation<T>(ICacheConfigurationItem<T> item) where T : class
            => base.GetCacheImplementation(item);
    }
}