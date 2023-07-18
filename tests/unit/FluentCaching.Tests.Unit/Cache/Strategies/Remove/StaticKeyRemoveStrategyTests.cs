using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Configuration;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Remove;

public class StaticKeyRemoveStrategyTests : BaseCacheStrategyTests
{
    private readonly StaticKeyRemoveStrategy<User> _sut;

    public StaticKeyRemoveStrategyTests()
    {
        _sut = new StaticKeyRemoveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RemoveAsync(CacheSource<User>.Create(null), CacheConfiguration.DefaultPolicyName);

        KeyBuilderMock
            .Verify(_ => _.BuildFromStaticKey<User>(), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromStaticKey<User>())
            .Returns(key);
            
        await _sut.RemoveAsync(CacheSource<User>.Create(null), CacheConfiguration.DefaultPolicyName);

        TypeCacheImplementationMock
            .Verify(_ => _.RemoveAsync(key), Times.Once);
    }
}