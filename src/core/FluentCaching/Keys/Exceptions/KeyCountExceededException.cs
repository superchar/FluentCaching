using System;

namespace FluentCaching.Keys.Exceptions;

public class KeyCountExceededException : Exception
{
    public KeyCountExceededException(int maxCount)
        : base(
            $"The caching key can consists of maximum '{maxCount}' parts. " +
            "Please change the configuration to reduce key parts count.")
    {
    }
}