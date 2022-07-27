using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache.Facades
{
    internal interface ICacheFacade 
    {
        Task CacheAsync<T>(T targetObject) where T : class;
        
        Task<T> RetrieveAsync<T>(object objectKey) where T : class;

        Task<T> RetrieveAsync<T>(string stringKey) where T : class;

        Task<T> RetrieveAsync<T>() where T : class;
        
        Task RemoveAsync<T>(object objectKey) where T : class;

        Task RemoveAsync<T>(string stringKey) where T : class;

        Task<TValue> RetrieveOrStoreAsync<TKey, TValue>(TKey key, Func<string, Task<TValue>> entityFetcher) where TValue : class;

        Task<T> RetrieveOrStoreAsync<T>(object objectKey, Func<object, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(Func<Task<T>> entityFetcher) where T : class;
    }
}