using System;
using FluentCaching.Extensions;

namespace FluentCaching.Keys.Exceptions;

public class KeyHasDynamicPartsException : Exception
{
    public KeyHasDynamicPartsException(Type type)
        : base($"Cannot build key without parameters, because key cache configuration for {type.ToFullNameString()} contains dynamic parts." +
               " Please provide necessary parameters when performing cache operations.")
    {
        
    }
}