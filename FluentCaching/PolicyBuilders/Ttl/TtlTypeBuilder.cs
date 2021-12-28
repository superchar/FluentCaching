﻿using System;
using FluentCaching.Cache.Models;
using FluentCaching.Keys;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class TtlTypeBuilder
    {
        private readonly CacheOptions _currentOptions = new CacheOptions();

        internal TtlTypeBuilder(IPropertyTracker propertyTracker)
        {
            _currentOptions.PropertyTracker = propertyTracker;
        }

        public TimeTtlBuilder WithTtlOf(short value) => new TimeTtlBuilder(_currentOptions, value);

        public AndBuilder<CacheImplementationBuilder> WithInfiniteTtl()
        {
            _currentOptions.Ttl = TimeSpan.MaxValue;
            return new AndBuilder<CacheImplementationBuilder>(new CacheImplementationBuilder(_currentOptions));
        }
    }
}
