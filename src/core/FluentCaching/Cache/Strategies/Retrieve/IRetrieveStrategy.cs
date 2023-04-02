using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache.Strategies.Retrieve;

internal interface IRetrieveStrategy<TEntity>
    where TEntity : class
{
    ValueTask<TEntity> RetrieveAsync(CacheSource<TEntity> source);
}