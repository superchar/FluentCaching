﻿

using System.Threading.Tasks;
using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class StoringHelperWrapper
    {
        private readonly CachingOptions _cachingOptions;

        public StoringHelperWrapper(CachingOptions cachingOptions)
        {
            _cachingOptions = cachingOptions;
        }

        public Task CacheAsync() => StoringHelper.StoreAsync(_cachingOptions);
    }
}