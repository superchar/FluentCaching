using System;

namespace FluentCaching.DistributedCache;

public class ServiceProviderNotInitializedException : Exception
{
    private const string ExceptionMessage =
        "Service provider is not initialized. To initialize it, please call '" +
        nameof(ServiceCollectionExtensions.UseFluentCaching) + "'.";

    public ServiceProviderNotInitializedException()
        : base(ExceptionMessage)
    {
    }
}