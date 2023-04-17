using System;
using FluentCaching.Cache.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DependencyInjectionExtensions;

// ReSharper disable once UnusedType.Global
public static class ServiceCollectionExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    // ReSharper disable once UnusedMember.Global
    public static IServiceCollection AddFluentCaching(this IServiceCollection serviceCollection, 
        Action<CacheBuilder> builderAction)
    {
        var builder = new CacheBuilder();
        builderAction(builder);
        var cache = builder.Build();
        serviceCollection.AddSingleton(cache);

        return serviceCollection;
    }
}