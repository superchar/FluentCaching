

using System;
using System.Threading.Tasks;
using FluentCaching.Configuration;
using FluentCaching.Parameters;

namespace FluentCaching
{
    internal static class CachingHelper
    {
        public static Task StoreAsync(CachingOptions cachingOptions)
        {
            var implementation = CachingConfiguration.Instance.Current;

            if (implementation == null)
            {
                throw new Exception("Cache implementation is not configured");
            }

            return implementation.SetAsync(cachingOptions);
        }
    }
}
