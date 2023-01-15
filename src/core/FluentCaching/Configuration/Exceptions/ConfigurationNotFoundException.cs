﻿using System;

namespace FluentCaching.Configuration.Exceptions;

public class ConfigurationNotFoundException : Exception
{
    public ConfigurationNotFoundException(Type type) 
        : base($"Caching configuration for type '{type.FullName}' is not found") 
    {
            
    }
}