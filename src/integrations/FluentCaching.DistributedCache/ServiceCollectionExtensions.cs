using Microsoft.AspNetCore.Builder;

namespace FluentCaching.DistributedCache
{
    public static class ServiceCollectionExtensions
    {
        public static IApplicationBuilder UseFluentCaching(this IApplicationBuilder builder)
        {
            ServiceLocator.Initialize(builder.ApplicationServices);

            return builder;
        }
    } 
}

