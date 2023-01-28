using System;

namespace FluentCaching.Keys.Exceptions;

public class KeyCountExceededException : Exception
{
    public KeyCountExceededException(int maxCount) 
        : base($"Maximum key parts count exceeded. Maximum count is {maxCount}.")
    {
            
    }
}