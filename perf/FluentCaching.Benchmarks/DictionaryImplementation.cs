using System.Collections.Generic;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.Benchmarks;

public class DictionaryImplementation : ICacheImplementation
{
    private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();

    public ValueTask<T> RetrieveAsync<T>(string key)
    {
        return new ValueTask<T>((T)_dictionary[key]);
    }

    public ValueTask CacheAsync<T>(string key, T targetObject, CacheOptions options)
    {
        _dictionary[key] = targetObject;
        return new ValueTask();
    }

    public ValueTask RemoveAsync(string key)
    {
        _dictionary.Remove(key);
        return new ValueTask();
    }

}