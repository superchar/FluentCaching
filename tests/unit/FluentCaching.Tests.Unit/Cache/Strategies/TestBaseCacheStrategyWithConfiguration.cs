using FluentCaching.Cache;
using FluentCaching.Cache.Strategies;
using FluentCaching.Configuration;

namespace FluentCaching.Tests.Unit.Cache.Strategies;

internal class TestBaseCacheStrategyWithConfiguration : BaseCacheStrategyWithConfiguration
{
    public TestBaseCacheStrategyWithConfiguration(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public new ICacheConfigurationItem GetConfigurationItem<T>() where T : class 
        => base.GetConfigurationItem<T>(CacheConfiguration.DefaultPolicyName);

    public new ICacheImplementation GetCacheImplementation<T>(ICacheConfigurationItem item) where T : class
        => base.GetCacheImplementation<T>(item);
}