using FluentCaching.Cache.Models;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    public interface ICacheImplementation
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T targetObject, CacheOptions options);

        Task RemoveAsync(string key);
    }
}
