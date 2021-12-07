using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache.Helpers
{
    internal interface IConcurrencyHelper
    {
        Task<T> ExecuteAsync<T>(string key, Func<string, Task<T>> callback) where T : class;

        Task<T> ExecuteAsync<T>(object key, Func<object, Task<T>> callback) where T : class;
    }
}