using FluentCaching.Cache;
using FluentCaching.Memory;

namespace FluentCaching.Tests.ThreadSafety;

// ReSharper disable once UnusedType.Global
public class MemoryCacheThreadSafetyTest : BaseCacheThreadSafetyTest
{
    protected override ICacheImplementation CacheImplementation => new MemoryCacheImplementation();

    protected override int UsersCount => 10_000;
}