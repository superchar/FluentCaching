using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Retrieve;

public class ScalarKeyRetrieveStrategyTests : BaseCacheStrategyTests
{
    private static readonly CacheSource<User> ScalarKeySource 
        = CacheSource<User>.Create("key part");

    private readonly ScalarKeyRetrieveStrategy<User> _sut;

    public ScalarKeyRetrieveStrategyTests()
    {
        _sut = new ScalarKeyRetrieveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RetrieveAsync(ScalarKeySource);

        KeyBuilderMock
            .Verify(_ => _.BuildFromScalarKey(ScalarKeySource.Key), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromScalarKey(ScalarKeySource.Key))
            .Returns(key);
            
        await _sut.RetrieveAsync(ScalarKeySource);

        TypeCacheImplementationMock
            .Verify(_ => _.RetrieveAsync<User>(key), Times.Once);
    }
}