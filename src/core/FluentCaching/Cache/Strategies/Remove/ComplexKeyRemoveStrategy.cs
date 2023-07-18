using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Remove;

internal class ComplexKeyRemoveStrategy<TEntity> : BaseCacheStrategyWithConfiguration, IRemoveStrategy<TEntity>
    where TEntity : class
{
    public ComplexKeyRemoveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public ValueTask RemoveAsync(CacheSource<TEntity> source, string policyName)
    {
        var item = GetConfigurationItem<TEntity>(policyName);
        var key = item.Options.KeyBuilder.BuildFromComplexKey(source.Key);
        return GetCacheImplementation<TEntity>(item)
            .RemoveAsync(key);
    }
}