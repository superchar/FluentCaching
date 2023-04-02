using System;
using FluentCaching.Extensions;

namespace FluentCaching.Keys.Exceptions;

public class KeyPropertyDuplicateException : Exception
{
    public KeyPropertyDuplicateException(string propertyName, Type type)
        : base($"The caching key for {type.ToFullNameString()} cannot contain multiple properties with the same name. " +
               $"Duplicated property name - '{propertyName}'.")
    {
        
    }
}