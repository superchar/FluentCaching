using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache.Strategies.Remove;

public interface IRemoveStrategy<T>
    where T : class
{
    ValueTask RemoveAsync(CacheSource<T> source);
}