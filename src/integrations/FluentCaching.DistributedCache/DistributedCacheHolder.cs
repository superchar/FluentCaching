using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DistributedCache;

public readonly struct DistributedCacheHolder : IDisposable
{
    private readonly IServiceScope? _serviceScope;
        
    public DistributedCacheHolder(IDistributedCache distributedCache)
    {
        DistributedCache = distributedCache;
        _serviceScope = null;
    }
    
    public DistributedCacheHolder()
    {
        _serviceScope = ServiceLocator.CreateScope();
        DistributedCache = _serviceScope.ServiceProvider.GetRequiredService<IDistributedCache>();
    }
    
    public IDistributedCache DistributedCache { get; }
        
    public void Dispose() => _serviceScope?.Dispose();
}