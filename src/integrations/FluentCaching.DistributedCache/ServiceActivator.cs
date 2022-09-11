﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCaching.DistributedCache
{
    public static class ServiceActivator
    {
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider) 
            => _serviceProvider = serviceProvider;

        public static IServiceScope CreateScope() =>
            _serviceProvider?.CreateScope() 
            ?? throw new InvalidOperationException("Service provider is not initialized.");
    }
}

