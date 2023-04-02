using System;
using FluentCaching.Cache.Builders;
using FluentCaching.Extensions;

namespace FluentCaching.Configuration.Exceptions;

public class ConfigurationNotFoundException : Exception
{
    public ConfigurationNotFoundException(Type type)
        : base(
            $"Cache configuration for {type.ToFullNameString()} is not found. " +
            $"Please use {nameof(CacheBuilder.For)} method to add cache configuration.")
    {
    }
}