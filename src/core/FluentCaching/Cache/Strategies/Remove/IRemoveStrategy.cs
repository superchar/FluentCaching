using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache.Strategies.Remove;

public interface IRemoveStrategy<T>
    where T : class
{
    Task RemoveAsync(CacheSource<T> source);
}