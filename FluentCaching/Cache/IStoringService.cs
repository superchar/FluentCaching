using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    internal interface IStoringService 
    {
        Task RemoveAsync<T>(object targetObject) where T : class;

        Task RemoveAsync<T>(string targetString) where T : class;

        Task<T> RetrieveAsync<T>(object targetObject) where T : class;

        Task<T> RetrieveAsync<T>(string targetString) where T : class;

        Task StoreAsync<T>(T targetObject) where T : class;

        Task<T> RetrieveOrStoreAsync<T>(string key, Func<string, Task<T>> entityFetcher) where T : class;

        Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, Task<T>> entityFetcher) where T : class;

        Task<T> RetrieveOrStoreAsync<T>(string key, Func<string, T> entityFetcher) where T : class;

        Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, T> entityFetcher) where T : class;
    }
}