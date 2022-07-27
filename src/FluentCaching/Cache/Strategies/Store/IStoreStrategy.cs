using System.Threading.Tasks;

namespace FluentCaching.Cache.Strategies.Store;

public interface IStoreStrategy<T>
    where T : class
{
    Task StoreAsync(T cachedObject);
}