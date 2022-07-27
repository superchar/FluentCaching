using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    public interface ICache
    {
        #region StoreAsync 
        
        Task CacheAsync<TEntity>(TEntity targetObject) where TEntity : class;

        #endregion StoreAsync

        #region RemoveAsync

        Task RemoveAsync<TEntity>(bool key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(byte key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(char key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(decimal key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(double key) where TEntity : class;
     
        Task RemoveAsync<TEntity>(float key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(int key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(long key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(sbyte key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(short key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(string key) where TEntity : class;
      
        Task RemoveAsync<TEntity>(uint key) where TEntity : class;
       
        Task RemoveAsync<TEntity>(ulong key) where TEntity : class;
        
        Task RemoveAsync<TEntity>(ushort key) where TEntity : class;

        Task RemoveAsync<TEntity>(object key) where TEntity : class;


        #endregion

        #region RetrieveAsync

        Task<TEntity> RetrieveAsync<TEntity>(bool key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(byte key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(char key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(decimal key) where TEntity : class;
       
        Task<TEntity> RetrieveAsync<TEntity>(double key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(float key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(int key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(long key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(object key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(sbyte key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(short key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(string key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(uint key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(ulong key) where TEntity : class;
        
        Task<TEntity> RetrieveAsync<TEntity>(ushort key) where TEntity : class;

        Task<TEntity> RetrieveAsync<TEntity>() where TEntity : class;

        #endregion

        #region RetrieveOrStoreAsync

        Task<T> RetrieveOrStoreAsync<T>(string key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(bool key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(int key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(byte key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(sbyte key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(char key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(decimal key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(double key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(float key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(long key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(ulong key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(short key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(ushort key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(uint key, Func<string, Task<T>> entityFetcher) where T : class;
        
        Task<T> RetrieveOrStoreAsync<T>(Func<Task<T>> entityFetcher) where T : class;

        #endregion
    }
}