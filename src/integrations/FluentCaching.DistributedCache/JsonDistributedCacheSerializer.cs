using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FluentCaching.DistributedCache;

public class JsonDistributedCacheSerializer : IDistributedCacheSerializer
{
    public ValueTask<byte[]> SerializeAsync<TEntity>(TEntity entity)
        => new(JsonSerializer.SerializeToUtf8Bytes(entity));

    public ValueTask<TEntity?> DeserializeAsync<TEntity>(byte[] bytes)
        => new(JsonSerializer.Deserialize<TEntity>(bytes));

    public bool CanBeUsedForType(Type type) => true;
}