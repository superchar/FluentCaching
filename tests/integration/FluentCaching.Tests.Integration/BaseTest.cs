using FluentCaching.Cache.Builders;
using FluentCaching.Tests.Integration.Fakes;

namespace FluentCaching.Tests.Integration;

public class BaseTest
{
    protected BaseTest()
    {
        CacheBuilder.SetGenericCache(CacheImplementation);
    }

    protected ICacheBuilder CacheBuilder { get; } = new CacheBuilder();

    protected DictionaryCacheImplementation CacheImplementation { get; } = new();
}