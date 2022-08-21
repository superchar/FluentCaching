using System;
using System.Threading.Tasks;

namespace FluentCaching.Cache.Facades
{
    internal interface ICacheFacade 
    {
        Task CacheAsync<T>(T targetObject) where T : class;
        
        Task<T> RetrieveComplexAsync<T>(object complexKey) where T : class;

        Task<T> RetrieveScalarAsync<T>(object stringKey) where T : class;

        Task<T> RetrieveStaticAsync<T>() where T : class;
        
        Task RemoveComplexAsync<T>(object objectKey) where T : class;

        Task RemoveScalarAsync<T>(object scalarKey) where T : class;
    }
}