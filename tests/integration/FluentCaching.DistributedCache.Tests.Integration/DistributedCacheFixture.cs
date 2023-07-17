using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace FluentCaching.DistributedCache.Tests.Integration;

public class DistributedCacheFixture : IDisposable
{
    private const int Port = 6379;

    private readonly RedisContainer _container = new RedisBuilder()
        .WithPortBinding(Port)
        .Build();

    private IDistributedCache? _cache;

    public DistributedCacheFixture()
    {
        _container.StartAsync().GetAwaiter().GetResult();
    }

    public IDistributedCache Cache => _cache ??= GetDistributedCache();
    
    public void Dispose()
    {
        _container.StopAsync().GetAwaiter().GetResult();
    }
    
    private static IDistributedCache GetDistributedCache()
    {
        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = {$"localhost:{Port}"},
            Ssl = false
        };

        return new ServiceCollection()
            .AddStackExchangeRedisCache(options => options.ConfigurationOptions = configurationOptions)
            .BuildServiceProvider()
            .GetRequiredService<IDistributedCache>();
    }
}
