using System.Collections.Generic;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;

namespace FluentCaching.Tests.Integration.Fakes
{
    public class DictionaryCacheImplementation : ICacheImplementation
    {
        public Dictionary<string, object> Dictionary { get; } = new Dictionary<string, object>();

        public ValueTask<T> RetrieveAsync<T>(string key)
        {
            return new ValueTask<T>((T)Dictionary.GetValueOrDefault(key));
        }

        public ValueTask RemoveAsync(string key)
        {
            Dictionary.Remove(key);
            return default;
        }

        public ValueTask CacheAsync<T>(string key, T targetObject, CacheOptions options)
        {
            Dictionary[key] = targetObject;
            return default;
        }
    }
}
