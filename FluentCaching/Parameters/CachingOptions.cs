﻿
using System;

namespace FluentCaching.Parameters
{
    public struct CachingOptions
    {
        public static readonly CachingOptions Default = new CachingOptions
        {
            Ttl = TimeSpan.MaxValue,
            ExpirationType = ExpirationType.Absolute,
            Key = string.Empty,
        };

        public TimeSpan Ttl { get; set; }

        public ExpirationType ExpirationType { get; set; }

        public string Key { get; set; }
    }
}