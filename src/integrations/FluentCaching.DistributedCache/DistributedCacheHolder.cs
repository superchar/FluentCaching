using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DistributedCache;

public readonly struct DistributedCacheHolder : IDisposable
{
    private readonly IServiceScope? _serviceScope;
        
    public DistributedCacheHolder(IDistributedCache cache)
    {
        Cache = cache;
        _serviceScope = null;
    }
    
    public DistributedCacheHolder()
    {
        _serviceScope = ServiceLocator.CreateScope();
        Cache = _serviceScope.ServiceProvider.GetRequiredService<IDistributedCache>();
    }
    
    public IDistributedCache Cache { get; }
        
    public void Dispose() => _serviceScope?.Dispose();
}