using System;
using FluentCaching.Extensions;

namespace FluentCaching.Keys.Exceptions;

public class KeyPartNullException : Exception
{
    public KeyPartNullException(Type type)
        : base($"Key part is null for {type.ToFullNameString()}. Please check the cache configuration.")
    {
        
    }
}