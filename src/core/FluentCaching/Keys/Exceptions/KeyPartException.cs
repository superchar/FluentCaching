using System;

namespace FluentCaching.Keys.Exceptions;

public class KeyPartException : Exception
{
    public KeyPartException(string message) : base(message)
    {
            
    }
}