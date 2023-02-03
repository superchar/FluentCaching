using FluentCaching.Cache;
using FluentCaching.Memory;

namespace FluentCaching.Tests.ThreadSafety
{
    public class MemoryCacheThreadSafetyTest : BaseCacheThreadSafetyTest
    {
        protected override ICacheImplementation CacheImplementation => new MemoryCacheImplementation();

        protected override int UsersCount => 10_000;
    }
}