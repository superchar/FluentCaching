using System.Threading.Tasks;

namespace FluentCaching.Cache.Strategies.Store;

public interface IStoreStrategy<in TEntity>
    where TEntity : class
{
    ValueTask StoreAsync(TEntity cachedObject);
}