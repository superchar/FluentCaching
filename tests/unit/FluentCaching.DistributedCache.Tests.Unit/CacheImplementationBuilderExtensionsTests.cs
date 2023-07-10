using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Keys.Builders;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace FluentCaching.DistributedCache.Tests.Unit;

public class CacheImplementationBuilderExtensionsTests
{
    [Fact]
    public void StoreInDistributedCache_CacheParameterIsNotProvided_ReturnsCacheImplementationPolicyBuilder()
    {
        var cacheImplementationPolicyBuilder = CreateCachePolicyBuilder();

        var result = cacheImplementationPolicyBuilder.StoreInDistributedCache();

        result.Should().NotBeNull();
    }

    [Fact]
    public void StoreInDistributedCache_CacheParameterIsProvided_ReturnsCacheImplementationPolicyBuilder()
    {
        var cacheImplementationPolicyBuilder = CreateCachePolicyBuilder();

        var result = cacheImplementationPolicyBuilder.StoreInDistributedCache(new Mock<IDistributedCache>().Object,
            new Mock<IDistributedCacheSerializer>().Object);

        result.Should().NotBeNull();
    }

    private static CacheImplementationPolicyBuilder CreateCachePolicyBuilder()
        => new (new CacheOptions(new Mock<IKeyBuilder>().Object));
}