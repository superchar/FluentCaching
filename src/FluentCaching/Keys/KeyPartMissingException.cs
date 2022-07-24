using System;

namespace FluentCaching.Keys
{
    public class KeyPartMissingException : Exception
    {
        public KeyPartMissingException() : base("The key part is missing")
        {
        }
    }
}
