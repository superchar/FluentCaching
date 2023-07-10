using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DistributedCache;

public readonly struct DistributedCacheSerializerHolder<T> : IDisposable
{
    private readonly IServiceScope? _serviceScope;

    private readonly IEnumerable<IDistributedCacheSerializer> _serializers;

    public DistributedCacheSerializerHolder(IEnumerable<IDistributedCacheSerializer> serializers)
    {
        _serializers = serializers;
        _serviceScope = null;
    }

    public DistributedCacheSerializerHolder()
    {
        _serviceScope = ServiceLocator.CreateScope();
        _serializers = _serviceScope.ServiceProvider.GetServices<IDistributedCacheSerializer>();
    }

    public IDistributedCacheSerializer Serializer =>
        _serializers.FirstOrDefault(s => s.CanBeUsedForType(typeof(T))) ?? new JsonDistributedCacheSerializer();

    public void Dispose() => _serviceScope?.Dispose();
}