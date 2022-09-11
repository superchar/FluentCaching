using System;
using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DependencyInjectionExtensions
{
    public static class FluentCachingExtensions
    {
        public static IServiceCollection AddFluentCaching(this IServiceCollection serviceCollection, 
            Action<CacheBuilder> builderAction)
        {
            var builder = new CacheBuilder();
            builderAction(builder);
            var cache = builder.Build();
            serviceCollection.AddSingleton<ICache>(cache);

            return serviceCollection;
        }
    }
}