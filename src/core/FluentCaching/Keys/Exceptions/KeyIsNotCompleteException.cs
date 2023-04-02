using System;
using FluentCaching.Extensions;

namespace FluentCaching.Keys.Exceptions;

public class KeyIsNotCompleteException : Exception
{
    public KeyIsNotCompleteException(Type type) 
        : base($"Cannot build cache key for {type.ToFullNameString()} as some key parts are missing." +
               "Please check cache configuration.")
    {
    }
}