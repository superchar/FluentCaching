using System.Collections.Generic;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.Tests.Integration.Fakes
{
    public class DictionaryCacheImplementation : ICacheImplementation
    {
        public Dictionary<string, object> Dictionary { get; set; } = new Dictionary<string, object>();

        public Task<T> RetrieveAsync<T>(string key)
        {
            return Task.FromResult((T)Dictionary.GetValueOrDefault(key));
        }

        public Task RemoveAsync(string key)
        {
            Dictionary.Remove(key);
            return Task.CompletedTask;
        }

        public Task CacheAsync<T>(string key, T targetObject, CacheOptions options)
        {
            Dictionary[key] = targetObject;
            return Task.CompletedTask;
        }
    }
}
