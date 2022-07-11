﻿using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache
{
    internal interface ICacheService 
    {
        Task RemoveAsync<T>(object targetObject) where T : class;

        Task RemoveAsync<T>(string targetString) where T : class;

        Task<T> RetrieveAsync<T>(object targetObject) where T : class;

        Task<T> RetrieveAsync<T>(string targetString) where T : class;

        Task<T> RetrieveAsync<T>() where T : class;

        Task CacheAsync<T>(T targetObject) where T : class;

        Task<TValue> RetrieveOrStoreAsync<TKey, TValue>(TKey key, Func<TKey, Task<TValue>> entityFetcher) where TValue : class;

        Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, Task<T>> entityFetcher) where T : class;

        Task<TValue> RetrieveOrStoreAsync<TKey, TValue>(TKey key, Func<TKey, TValue> entityFetcher) where TValue : class;

        Task<T> RetrieveOrStoreAsync<T>(object key, Func<object, T> entityFetcher) where T : class;

        Task<T> RetrieveOrStoreAsync<T>(Func<T> entityFetcher) where T : class;
    }
}