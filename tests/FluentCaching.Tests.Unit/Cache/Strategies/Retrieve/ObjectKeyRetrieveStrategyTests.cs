using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.Retrieve;

public class ObjectKeyRetrieveStrategyTests : BaseCacheStrategyTests
{
    private static readonly CacheSource<User> ObjectKeySource = new(new object());
    
    private readonly ObjectKeyRetrieveStrategy<User> _sut;

    public ObjectKeyRetrieveStrategyTests()
    {
        _sut = new ObjectKeyRetrieveStrategy<User>(CacheConfigurationMock.Object);
    }
    
    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsKeyBuilder()
    {
        await _sut.RetrieveAsync(ObjectKeySource);

        KeyBuilderMock
            .Verify(_ => _.BuildFromObjectKey(ObjectKeySource.ObjectKey), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_WhenCalled_CallsCacheImplementation()
    {
        const string key = "key";
        KeyBuilderMock
            .Setup(_ => _.BuildFromObjectKey(ObjectKeySource.ObjectKey))
            .Returns(key);
            
        await _sut.RetrieveAsync(ObjectKeySource);

        TypeCacheImplementationMock
            .Verify(_ => _.RetrieveAsync<User>(key), Times.Once);
    }
}