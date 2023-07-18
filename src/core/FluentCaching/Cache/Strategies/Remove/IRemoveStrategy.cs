using System.Threading.Tasks;
using FluentCaching.Cache.Models;

namespace FluentCaching.Cache.Strategies.Remove;

public interface IRemoveStrategy<TEntity>
    where TEntity : class
{
    ValueTask RemoveAsync(CacheSource<TEntity> source, string policyName);
}