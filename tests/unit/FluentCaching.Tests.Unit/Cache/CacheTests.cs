using System.Threading.Tasks;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache;

public class CacheTests
{
    private const string ScalarKey = "key";

    private static readonly User CachedObject = new();
    private static readonly object ComplexKey = new();

    private readonly Mock<IStoreStrategy<User>> _storeStrategyMock;
    private readonly Mock<IRetrieveStrategy<User>> _retrieveStrategyMock;
    private readonly Mock<IRemoveStrategy<User>> _removeStrategyMock;
    private readonly Mock<ICacheStrategyFactory> _cacheStrategyFactoryMock;

    private readonly FluentCaching.Cache.Cache _sut;

    public CacheTests()
    {
        _cacheStrategyFactoryMock = new Mock<ICacheStrategyFactory>();
        _storeStrategyMock = new Mock<IStoreStrategy<User>>();
        _retrieveStrategyMock = new Mock<IRetrieveStrategy<User>>();
        _removeStrategyMock = new Mock<IRemoveStrategy<User>>();

        _cacheStrategyFactoryMock
            .Setup(_ => _.CreateStoreStrategy<User>())
            .Returns(_storeStrategyMock.Object);
        _cacheStrategyFactoryMock
            .Setup(_ => _.CreateRetrieveStrategy(It.IsAny<CacheSource<User>>()))
            .Returns(_retrieveStrategyMock.Object);
        _cacheStrategyFactoryMock
            .Setup(_ => _.CreateRemoveStrategy(It.IsAny<CacheSource<User>>()))
            .Returns(_removeStrategyMock.Object);

        _sut = new FluentCaching.Cache.Cache(_cacheStrategyFactoryMock.Object);
    }

    [Fact]
    public async Task CacheAsync_WhenCalled_CallsCacheStrategyFactory()
    {
        await _sut.CacheAsync(CachedObject);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateStoreStrategy<User>(), Times.Once);
    }

    [Fact]
    public async Task CacheAsync_WhenCalled_CallsStoreStrategy()
    {
        await _sut.CacheAsync(CachedObject);

        _storeStrategyMock
            .Verify(_ => _.StoreAsync(CachedObject), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_ComplexKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveAsync<User>(ComplexKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy(
                    It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_ComplexKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveAsync<User>(ComplexKey);

        _retrieveStrategyMock
            .Verify(_ =>
                    _.RetrieveAsync(It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_ScalarKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveAsync<User>(ScalarKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy(
                    It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_ScalarKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveAsync<User>(ScalarKey);

        _retrieveStrategyMock
            .Verify(_ =>
                    _.RetrieveAsync(It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_StaticKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveAsync<User>();

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy(It.Is<CacheSource<User>>(s => s.CacheSourceType == CacheSourceType.Static)), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_StaticKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveAsync<User>();

        _retrieveStrategyMock
            .Verify(_ => _.RetrieveAsync(It.Is<CacheSource<User>>(s => s.CacheSourceType == CacheSourceType.Static)), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ComplexKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveAsync<User>(ComplexKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy(
                    It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ComplexKey_CallsRemoveStrategy()
    {
        await _sut.RemoveAsync<User>(ComplexKey);

        _removeStrategyMock
            .Verify(_ =>
                    _.RemoveAsync(It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ScalarKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveAsync<User>(ScalarKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy(
                    It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ScalarKey_CallsRemoveStrategy()
    {
        await _sut.RemoveAsync<User>(ScalarKey);

        _removeStrategyMock
            .Verify(_ =>
                    _.RemoveAsync(It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_StaticKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveAsync<User>();

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy(It.Is<CacheSource<User>>(s => s.CacheSourceType == CacheSourceType.Static)), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_StaticKey_CallsRemoveStrategy()
    {
        await _sut.RemoveAsync<User>();

        _removeStrategyMock
            .Verify(_ => _.RemoveAsync(It.Is<CacheSource<User>>(s => s.CacheSourceType == CacheSourceType.Static)), Times.Once);
    }
}