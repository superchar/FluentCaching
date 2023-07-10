using System;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DistributedCache;

public static class ServiceLocator
{
    private static IServiceProvider? _serviceProvider;

    public static void Initialize(IServiceProvider? serviceProvider)
        => _serviceProvider = serviceProvider;

    public static IServiceScope CreateScope() =>
        _serviceProvider?.CreateScope()
        ?? throw new ServiceProviderNotInitializedException();
}