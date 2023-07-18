using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal class ComplexKeyRetrieveStrategy<TEntity> : BaseCacheStrategyWithConfiguration, IRetrieveStrategy<TEntity>
    where TEntity : class
{
    public ComplexKeyRetrieveStrategy(ICacheConfiguration configuration) : base(configuration)
    {
    }

    public ValueTask<TEntity?> RetrieveAsync(CacheSource<TEntity> source, string policyName)
    {
        var item = GetConfigurationItem<TEntity>(policyName);
        var key = item.Options.KeyBuilder.BuildFromComplexKey(source.Key);
        return GetCacheImplementation<TEntity>(item)
            .RetrieveAsync<TEntity>(key);
    }
}