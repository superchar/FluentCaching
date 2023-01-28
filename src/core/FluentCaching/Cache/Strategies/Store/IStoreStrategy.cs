using System.Threading.Tasks;

namespace FluentCaching.Cache.Strategies.Store;

public interface IStoreStrategy<in T>
    where T : class
{
    ValueTask StoreAsync(T cachedObject);
}