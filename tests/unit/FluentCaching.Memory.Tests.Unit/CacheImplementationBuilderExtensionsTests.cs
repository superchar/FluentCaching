using FluentAssertions;
using FluentCaching.Cache.Builders;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Keys.Builders;
using Moq;
using Xunit;

namespace FluentCaching.Memory.Tests.Unit;

public class CacheImplementationBuilderExtensionsTests 
{
    [Fact]
    public void StoreInMemory_WhenCalled_ReturnsCacheImplementationPolicyBuilder()
    {
        var builder = new CacheImplementationPolicyBuilder(new CacheOptions(new Mock<IKeyBuilder>().Object));

        var result = builder.StoreInMemory();

        result.Should().NotBeNull();
    }
    
    [Fact]
    public void StoreInMemory_WhenCalled_SetsMemoryCacheAsDefault()
    {
        var configurationMock = new Mock<ICacheConfiguration>();
        var builder = new CacheBuilder(configurationMock.Object);

        builder.SetInMemoryAsDefaultCache();

        configurationMock
            .Verify(c => c.SetGenericCache(It.IsAny<MemoryCacheImplementation>()), Times.Once);
    }
}