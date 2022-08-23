using System;
using System.Threading.Tasks;
using FluentCaching.Cache.Facades;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Factories;
using FluentCaching.Cache.Strategies.Remove;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Facades;

public class CacheFacadeTests
{
    private static readonly User CachedObject = new();
    private static readonly object ComplexKey = new();
    private static readonly string ScalarKey = "key";

    private readonly Mock<IStoreStrategy<User>> _storeStrategyMock;
    private readonly Mock<IRetrieveStrategy<User>> _retrieveStrategyMock;
    private readonly Mock<IRemoveStrategy<User>> _removeStrategyMock;
    private readonly Mock<ICacheStrategyFactory> _cacheStrategyFactoryMock;

    private readonly CacheFacade _sut;

    public CacheFacadeTests()
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
    public async Task RetrieveComplexAsync_ObjectKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveComplexAsync<User>(ComplexKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy(
                    It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveComplexAsync_ObjectKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveComplexAsync<User>(ComplexKey);

        _retrieveStrategyMock
            .Verify(_ =>
                    _.RetrieveAsync(It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveScalarAsync_StringKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveScalarAsync<User>(ScalarKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy(
                    It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveScalarAsync_StringKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveScalarAsync<User>(ScalarKey);

        _retrieveStrategyMock
            .Verify(_ =>
                    _.RetrieveAsync(It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }

    [Fact]
    public async Task RetrieveStaticAsync_StaticKey_CallsCacheStrategyFactory()
    {
        await _sut.RetrieveStaticAsync<User>();

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRetrieveStrategy(CacheSource<User>.Static), Times.Once);
    }

    [Fact]
    public async Task RetrieveStaticAsync_StaticKey_CallsRetrieveStrategy()
    {
        await _sut.RetrieveStaticAsync<User>();

        _retrieveStrategyMock
            .Verify(_ => _.RetrieveAsync(CacheSource<User>.Static), Times.Once);
    }

    [Fact]
    public async Task RemoveComplexAsync_ObjectKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveComplexAsync<User>(ComplexKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy(
                    It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RemoveComplexAsync_ObjectKey_CallsRemoveStrategy()
    {
        await _sut.RemoveComplexAsync<User>(ComplexKey);

        _removeStrategyMock
            .Verify(_ =>
                    _.RemoveAsync(It.Is<CacheSource<User>>(s => s.Key == ComplexKey)),
                Times.Once);
    }

    [Fact]
    public async Task RemoveScalarAsync_StringKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveScalarAsync<User>(ScalarKey);

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy(
                    It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }

    [Fact]
    public async Task RemoveScalarAsync_StringKey_CallsRemoveStrategy()
    {
        await _sut.RemoveScalarAsync<User>(ScalarKey);

        _removeStrategyMock
            .Verify(_ =>
                    _.RemoveAsync(It.Is<CacheSource<User>>(s => ScalarKey.Equals(s.Key))),
                Times.Once);
    }
    
    [Fact]
    public async Task RemoveStaticAsync_StaticKey_CallsCacheStrategyFactory()
    {
        await _sut.RemoveStaticAsync<User>();

        _cacheStrategyFactoryMock
            .Verify(_ => _.CreateRemoveStrategy(CacheSource<User>.Static), Times.Once);
    }

    [Fact]
    public async Task RemoveStaticAsync_StaticKey_CallsRemoveStrategy()
    {
        await _sut.RemoveStaticAsync<User>();

        _removeStrategyMock
            .Verify(_ => _.RemoveAsync(CacheSource<User>.Static), Times.Once);
    }
}