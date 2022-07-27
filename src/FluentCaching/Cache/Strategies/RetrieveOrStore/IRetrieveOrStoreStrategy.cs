using System;
using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache.Strategies.RetrieveOrStore;

internal interface IRetrieveOrStoreStrategy<T>
    where T : class
{
    Task<T> RetrieveOrStoreAsync(CacheSource<T> source, Func<CacheSource<T>, Task<T>> entityFetcher);
}