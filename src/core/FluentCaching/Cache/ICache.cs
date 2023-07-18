using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;

// ReSharper disable UnusedMember.Global

namespace FluentCaching.Cache;

public interface ICache
{
    ValueTask CacheAsync<TEntity>(TEntity targetObject, PolicyName? policyName = null) where TEntity : class;
        
    ValueTask<TEntity?> RetrieveAsync<TEntity>(object key, PolicyName? policyName = null) where TEntity : class;
        
    ValueTask<TEntity?> RetrieveAsync<TEntity>(PolicyName? policyName = null) where TEntity : class;
        
    ValueTask RemoveAsync<TEntity>(object key, PolicyName? policyName = null) where TEntity : class;

    ValueTask RemoveAsync<TEntity>(PolicyName? policyName = null) where TEntity : class;
}