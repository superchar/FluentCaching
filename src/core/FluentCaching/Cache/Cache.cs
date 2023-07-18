using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;
using FluentCaching.Configuration;

namespace FluentCaching.Cache;

internal class Cache : ICache
{
    private readonly ICacheStrategyFactory _cacheStrategyFactory;

    public Cache(ICacheStrategyFactory cacheStrategyFactory)
    {
        _cacheStrategyFactory = cacheStrategyFactory;
    }

    public ValueTask CacheAsync<TEntity>(TEntity targetObject, PolicyName? policyName = null) where TEntity : class
        => _cacheStrategyFactory
            .CreateStoreStrategy<TEntity>()
            .StoreAsync(targetObject, GetPolicyNameString(policyName));

    public ValueTask<TEntity?> RetrieveAsync<TEntity>(object key, PolicyName? policyName = null) where TEntity : class
        => RetrieveAsync(CacheSource<TEntity>.Create(key), policyName);

    public ValueTask<TEntity?> RetrieveAsync<TEntity>(PolicyName? policyName = null) where TEntity : class
        => RetrieveAsync(CacheSource<TEntity>.Create(null), policyName);

    public ValueTask RemoveAsync<TEntity>(object key, PolicyName? policyName = null) where TEntity : class
        => RemoveAsync(CacheSource<TEntity>.Create(key), policyName);

    public ValueTask RemoveAsync<TEntity>(PolicyName? policyName = null) where TEntity : class
        => RemoveAsync(CacheSource<TEntity>.Create(null), policyName);
        
    private ValueTask<TEntity?> RetrieveAsync<TEntity>(CacheSource<TEntity> source, PolicyName? policyName) where TEntity : class
        => _cacheStrategyFactory
            .CreateRetrieveStrategy(source)
            .RetrieveAsync(source, GetPolicyNameString(policyName));
        
    private ValueTask RemoveAsync<TEntity>(CacheSource<TEntity> source, PolicyName? policyName) where TEntity : class
        => _cacheStrategyFactory
            .CreateRemoveStrategy(source)
            .RemoveAsync(source, GetPolicyNameString(policyName));
    
    private static string GetPolicyNameString(PolicyName? policyName) =>
        policyName?.Name ?? CacheConfiguration.DefaultPolicyName;
}