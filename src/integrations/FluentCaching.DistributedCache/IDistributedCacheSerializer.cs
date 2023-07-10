using System;
using System.Threading.Tasks;

namespace FluentCaching.DistributedCache;

public interface IDistributedCacheSerializer
{
    ValueTask<byte[]> SerializeAsync<TEntity>(TEntity entity);

    ValueTask<TEntity?> DeserializeAsync<TEntity>(byte[] bytes);

    bool CanBeUsedForType(Type type);
}