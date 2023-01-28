using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache;

public interface ICacheImplementation
{
    ValueTask<T> RetrieveAsync<T>(string key);

    ValueTask CacheAsync<T>(string key, T targetObject, CacheOptions options);

    ValueTask RemoveAsync(string key);
}