using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace FluentCaching.Cache;

public interface ICache
{
    ValueTask CacheAsync<TEntity>(TEntity targetObject) where TEntity : class;
        
    ValueTask<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class;
        
    ValueTask<TEntity> RetrieveAsync<TEntity>() where TEntity : class;
        
    ValueTask RemoveAsync<TEntity>(object key) where TEntity : class;

    ValueTask RemoveAsync<TEntity>() where TEntity : class;
}