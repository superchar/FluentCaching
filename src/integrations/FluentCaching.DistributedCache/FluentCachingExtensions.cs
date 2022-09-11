using Microsoft.AspNetCore.Builder;

namespace FluentCaching.DistributedCache
{
    public static class FluentCachingExtensions
    {
        public static IApplicationBuilder UseFluentCaching(this IApplicationBuilder builder)
        {
            ServiceActivator.Initialize(builder.ApplicationServices);

            return builder;
        }
    } 
}

