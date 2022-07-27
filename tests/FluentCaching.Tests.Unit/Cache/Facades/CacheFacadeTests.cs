using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentCaching.Cache.Facades;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.RetrieveOrStore;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Facades;

public class CacheFacadeTests
{
    private static readonly User CachedObject = new();
    private static readonly object ObjectKey = new();
    private static readonly string StringKey = "key";

    private readonly Mock<IStoreStrategy<User>> _storeStrategyMock;
    private readonly Mock<IRetrieveStrategy<User>> _retrieveStrategyMock;
    private readonly Mock<IRemoveStrategy<User>> _removeStrategyMock;
    private readonly Mock<IRetrieveOrStoreStrategy<User>> _retrieveOrStoreStrategyMock;
    private readonly Mock<ICacheStrategyFactory> _cacheStrategyFactoryMock;

    private readonly CacheFacade _sut;

    public CacheFacadeTests()
    {
        _cacheStrategyFactoryMock = new Mock<ICacheStrategyFactory>();
        _storeStrategyMock = new Mock<IStoreStrategy<User>>();
        _retrieveStrategyMock = new Mock<IRetrieveStrategy<User>>();
        _removeStrategyMock = new Mock<IRemoveStrategy<User>>();
        _retrieveOrStoreStrategyMock = new Mock<IRetrieveOrStoreStrategy<User>>();

        _cacheStrategyFactoryMock
            .Setup(_ => _.CreateStoreStrategy<User>())
            .Returns(_storeStrategyMock.Object);
        _cacheStrategyFactoryMock
            .Setup(_ => _.CreateRetrieveStrategy(It.IsAny<CacheSource<User>>()))
            .Returns(_retrieveStrategyMock.Object);
        _cacheStrategyFactoryMock
            .Setup(_ => _.CreateRemoveStrategy(It.IsAny<CacheSource<User>>()))
            .Returns(_removeStrategyMock.Object);
        _cacheStrategyFactoryMock
            .Setup(_ => _.CreateRetrieveOrStoreStrategy(It.IsAny<CacheSource<User>>()))
            .Returns(_retrieveOrStoreStrategyMock.Object);

        _sut = new CacheFacade(_cacheStrategyFactoryMock.Object);
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
    public async Task RetrieveAsync_ObjectKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveAsync<User>(ObjectKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy<User>(
                    It.Is<CacheSource<User>>(s => s.ObjectKey == ObjectKey)),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_ObjectKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveAsync<User>(ObjectKey);

        _retrieveStrategyMock
            .Verify(_ =>
                    _.RetrieveAsync(It.Is<CacheSource<User>>(s => s.ObjectKey == ObjectKey)),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_StringKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveAsync<User>(StringKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy<User>(
                    It.Is<CacheSource<User>>(s => StringKey.Equals(s.StringKey))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_StringKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveAsync<User>(StringKey);

        _retrieveStrategyMock
            .Verify(_ =>
                    _.RetrieveAsync(It.Is<CacheSource<User>>(s => StringKey.Equals(s.StringKey))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_StaticKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveAsync<User>();

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy<User>(CacheSource<User>.Null), Times.Once);
    }

    [Fact]
    public async Task RetrieveAsync_StaticKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveAsync<User>();

        _retrieveStrategyMock
            .Verify(_ => _.RetrieveAsync(CacheSource<User>.Null), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ObjectKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveAsync<User>(ObjectKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy<User>(
                    It.Is<CacheSource<User>>(s => s.ObjectKey == ObjectKey)),
                Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ObjectKey_CallsRemoveStrategy()
    {
        await _sut.RemoveAsync<User>(ObjectKey);

        _removeStrategyMock
            .Verify(_ =>
                    _.RemoveAsync(It.Is<CacheSource<User>>(s => s.ObjectKey == ObjectKey)),
                Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_StringKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveAsync<User>(StringKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy<User>(
                    It.Is<CacheSource<User>>(s => StringKey.Equals(s.StringKey))),
                Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_StringKey_CallsRemoveStrategy()
    {
        await _sut.RemoveAsync<User>(StringKey);

        _removeStrategyMock
            .Verify(_ =>
                    _.RemoveAsync(It.Is<CacheSource<User>>(s => StringKey.Equals(s.StringKey))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveOrStoreAsync_ObjectKey_CallsCacheStrategyFactory()
    {
        var entityFetcher = new Mock<Func<object, Task<User>>>().Object;

        await _sut.RetrieveOrStoreAsync(ObjectKey, entityFetcher);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveOrStoreStrategy<User>(
                    It.Is<CacheSource<User>>(s => s.ObjectKey == ObjectKey)),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveOrStoreAsync_ObjectKey_CallsRetrieveStrategy()
    {
        var entityFetcher = new Mock<Func<object, Task<User>>>().Object;

        await _sut.RetrieveOrStoreAsync(ObjectKey, entityFetcher);

        _retrieveOrStoreStrategyMock
            .Verify(_ =>
                    _.RetrieveOrStoreAsync(
                        It.Is<CacheSource<User>>(s => s.ObjectKey == ObjectKey),
                        It.IsAny<Func<CacheSource<User>, Task<User>>>()),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveOrStoreAsync_StringKey_CallsCacheStrategyFactory()
    {
        var entityFetcher = new Mock<Func<string, Task<User>>>().Object;

        await _sut.RetrieveOrStoreAsync(StringKey, entityFetcher);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveOrStoreStrategy<User>(
                    It.Is<CacheSource<User>>(s => StringKey.Equals(s.StringKey))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveOrStoreAsync_StringKey_CallsRetrieveStrategy()
    {
        var entityFetcher = new Mock<Func<string, Task<User>>>().Object;

        await _sut.RetrieveOrStoreAsync(StringKey, entityFetcher);

        _retrieveOrStoreStrategyMock
            .Verify(_ =>
                    _.RetrieveOrStoreAsync(
                        It.Is<CacheSource<User>>(s => StringKey.Equals(s.StringKey)),
                        It.IsAny<Func<CacheSource<User>, Task<User>>>()),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveOrStoreAsync_StaticKey_CallsCacheStrategyFactory()
    {
        var entityFetcher = new Mock<Func<Task<User>>>().Object;

        await _sut.RetrieveOrStoreAsync(entityFetcher);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveOrStoreStrategy<User>(CacheSource<User>.Null), Times.Once);
    }

    [Fact]
    public async Task RetrieveOrStoreAsync_StaticKey_CallsRetrieveStrategy()
    {
        var entityFetcher = new Mock<Func<Task<User>>>().Object;

        await _sut.RetrieveOrStoreAsync(entityFetcher);

        _retrieveOrStoreStrategyMock
            .Verify(_ => _.RetrieveOrStoreAsync(
                    CacheSource<User>.Null, It.IsAny<Func<CacheSource<User>, Task<User>>>()),
                Times.Once);
    }
}