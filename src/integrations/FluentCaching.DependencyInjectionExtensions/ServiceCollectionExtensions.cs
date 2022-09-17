using System;
using FluentCaching.Cache.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DependencyInjectionExtensions
{
    public static class ServiceCollectionExtensions
    {
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
}