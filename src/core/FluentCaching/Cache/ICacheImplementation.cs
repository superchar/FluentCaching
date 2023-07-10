using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache;

public interface ICacheImplementation
{
    ValueTask<TEntity?> RetrieveAsync<TEntity>(string key);

    ValueTask CacheAsync<TEntity>(string key, TEntity entity, CacheOptions options)
        where TEntity : notnull;

    ValueTask RemoveAsync(string key);
}