

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentCaching.Parameters;

namespace FluentCaching.Tests.Mocks
{
    public class DictionaryCacheImplementation : ICacheImplementation
    {
        public Dictionary<string, object> Dictionary { get; set; } = new Dictionary<string, object>();

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult((T)Dictionary[key]);
        }

        public Task SetAsync<T>(string key, T targetObject, CachingOptions options)
        {
            Dictionary[key] = targetObject;
            return Task.CompletedTask;
        }
    }
}
