using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal interface IRetrieveStrategy<T>
    where T : class
{
    ValueTask<T> RetrieveAsync(CacheSource<T> source);
}