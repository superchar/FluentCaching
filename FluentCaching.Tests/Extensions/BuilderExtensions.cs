﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentCaching.Api;
using FluentCaching.Api.Keys;

namespace FluentCaching.Tests.Extensions
{
    public static class BuilderExtensions
    {
        public static ExpirationBuilder Complete<T>(this CombinedCachingKeyBuilder<T> builder)
            where T : class
        {
            return builder.And().WithTtlOf(5).Seconds.And().SlidingExpiration();
        }
    }
}
