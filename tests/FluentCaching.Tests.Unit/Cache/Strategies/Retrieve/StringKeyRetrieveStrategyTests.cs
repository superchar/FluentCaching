using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Retrieve;

public class StringKeyRetrieveStrategyTests : BaseCacheStrategyTests
{
    private static readonly CacheSource<User> StringKeySource = new("key part");

    private readonly StringKeyRetrieveStrategy<User> _sut;

    public StringKeyRetrieveStrategyTests()
    {
        _sut = new StringKeyRetrieveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RetrieveAsync(StringKeySource);

        KeyBuilderMock
            .Verify(_ => _.BuildFromStringKey(StringKeySource.StringKey), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromStringKey(StringKeySource.StringKey))
            .Returns(key);
            
        await _sut.RetrieveAsync(StringKeySource);

        TypeCacheImplementationMock
            .Verify(_ => _.RetrieveAsync<User>(key), Times.Once);
    }
}