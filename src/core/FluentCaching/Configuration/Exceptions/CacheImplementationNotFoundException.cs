using System;
using FluentCaching.Configuration.PolicyBuilders;

namespace FluentCaching.Configuration.Exceptions;

public class CacheImplementationNotFoundException : Exception
{
    public CacheImplementationNotFoundException(Type type)
        : base(
            $"No cache implementation configured for type - '{type.FullName}'. " +
            $"Please use {nameof(CacheImplementationPolicyBuilder.StoreIn)} or {nameof(ICacheConfiguration.SetGenericCache)} method to configure it.")
    {
    }
}