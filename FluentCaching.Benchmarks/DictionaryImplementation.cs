using FluentCaching.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentCaching.Benchmarks
{
    public class DictionaryImplementation : ICacheImplementation
    {
        private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult((T)_dictionary[key]);
        }

        public Task SetAsync<T>(T targetObject, CachingOptions options)
        {
            _dictionary[options.Key] = targetObject;
            return Task.CompletedTask;
        }
    }
}
