using System.Collections.Generic;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.Benchmarks
{
    public class DictionaryImplementation : ICacheImplementation
    {
        private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();

        public Task<T> RetrieveAsync<T>(string key)
        {
            return Task.FromResult((T)_dictionary[key]);
        }

        public Task CacheAsync<T>(string key, T targetObject, CacheOptions options)
        {
            _dictionary[key] = targetObject;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _dictionary.Remove(key);
            return Task.CompletedTask;
        }

    }
}
