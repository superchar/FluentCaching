using System;

namespace FluentCaching.Exceptions
{
    public class KeyPartNullException : Exception
    {
        public KeyPartNullException() : base("A part of a caching key cannot be null")
        {
            
        }
    }
}
