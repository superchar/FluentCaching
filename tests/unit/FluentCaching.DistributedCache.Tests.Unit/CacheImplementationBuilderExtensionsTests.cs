using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace FluentCaching.DistributedCache.Tests.Unit;

public class CacheImplementationBuilderExtensionsTests
{
    [Fact]
    public void StoreInDistributedCache_CacheParameterIsNotProvided_ReturnsCacheImplementationPolicyBuilder()
    {
        var cacheImplementationPolicyBuilder = new CacheImplementationPolicyBuilder(new CacheOptions());

        var result = cacheImplementationPolicyBuilder.StoreInDistributedCache();

        result.Should().NotBeNull();
    }

    [Fact]
    public void StoreInDistributedCache_CacheParameterIsProvided_ReturnsCacheImplementationPolicyBuilder()
    {
        var cacheImplementationPolicyBuilder = new CacheImplementationPolicyBuilder(new CacheOptions());

        var result = cacheImplementationPolicyBuilder.StoreInDistributedCache(new Mock<IDistributedCache>().Object);

        result.Should().NotBeNull();
    }
}