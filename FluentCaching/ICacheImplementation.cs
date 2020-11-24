
using System.Threading.Tasks;
using FluentCaching.Parameters;

namespace FluentCaching
{
    public interface ICacheImplementation
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(T targetObject, CachingOptions options);
    }
}
