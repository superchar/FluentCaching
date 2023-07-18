using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Configuration;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Retrieve;

public class StaticKeyRetrieveStrategyTests : BaseCacheStrategyTests
{
    private readonly StaticKeyRetrieveStrategy<User> _sut;

    public StaticKeyRetrieveStrategyTests()
    {
        _sut = new StaticKeyRetrieveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RetrieveAsync(CacheSource<User>.Create(null), CacheConfiguration.DefaultPolicyName);

        KeyBuilderMock
            .Verify(_ => _.BuildFromStaticKey<User>(), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromStaticKey<User>())
            .Returns(key);
            
        await _sut.RetrieveAsync(CacheSource<User>.Create(null), CacheConfiguration.DefaultPolicyName);

        TypeCacheImplementationMock
            .Verify(_ => _.RetrieveAsync<User>(key), Times.Once);
    }
}