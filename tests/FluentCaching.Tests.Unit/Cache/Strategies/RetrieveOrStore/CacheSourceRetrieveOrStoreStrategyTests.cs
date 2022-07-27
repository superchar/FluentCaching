using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Cache.Strategies.Retrieve;
using FluentCaching.Cache.Strategies.RetrieveOrStore;
using FluentCaching.Cache.Strategies.Store;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Strategies.RetrieveOrStore;

public class CacheSourceRetrieveOrStoreStrategyTests
{
    private static readonly CacheSource<User> CacheSource = new(new object());

    private readonly Mock<IStoreStrategy<User>> _storeStrategyMock;
    private readonly Mock<IRetrieveStrategy<User>> _retrieveStrategyMock;
    private readonly Mock<Func<CacheSource<User>, Task<User>>> _entityFetcherMock;

    private readonly CacheSourceRetrieveOrStoreStrategy<User> _sut;

    public CacheSourceRetrieveOrStoreStrategyTests()
    {
        _storeStrategyMock = new Mock<IStoreStrategy<User>>();
        _retrieveStrategyMock = new Mock<IRetrieveStrategy<User>>();
        _entityFetcherMock = new Mock<Func<CacheSource<User>, Task<User>>>();

        _sut = new CacheSourceRetrieveOrStoreStrategy<User>(
            _storeStrategyMock.Object, 
            _retrieveStrategyMock.Object);
    }

    [Fact]
    public async Task RetrieveOrStoreAsync_RetrievedValueIsNotNull_ReturnsRetrievedValue()
    {
        var user = SetupRetrievedUser();
        var result = await _sut.RetrieveOrStoreAsync(CacheSource, _entityFetcherMock.Object);

        result.Should().BeSameAs(user);
    }
    
    [Fact]
    public async Task RetrieveOrStoreAsync_RetrievedValueIsNotNull_DoesNotCallEntityFetcher()
    {
        SetupRetrievedUser();
        
        var result = await _sut.RetrieveOrStoreAsync(CacheSource, _entityFetcherMock.Object);
        
        _entityFetcherMock
            .Verify(f => f(It.IsAny<CacheSource<User>>()), Times.Never);
    }
    
    [Fact]
    public async Task RetrieveOrStoreAsync_RetrievedValueIsNotNull_DoesNotCallStoreStrategy()
    {
        SetupRetrievedUser();
        
        var result = await _sut.RetrieveOrStoreAsync(CacheSource, _entityFetcherMock.Object);
        
        _storeStrategyMock
            .Verify(_ => _.StoreAsync(It.IsAny<User>()), Times.Never);
    }
    
    [Fact]
    public async Task RetrieveOrStoreAsync_RetrievedValueIsNull_CallsEntityFetcher()
    {
        var result = await _sut.RetrieveOrStoreAsync(CacheSource, _entityFetcherMock.Object);
        
        _entityFetcherMock
            .Verify(f => f(CacheSource), Times.Once);
    }
    
    [Fact]
    public async Task RetrieveOrStoreAsync_RetrievedValueIsNull_ReturnsValueFromEntityFetcher()
    {
        var user = SetupEntityFetcherUser();
        
        var result = await _sut.RetrieveOrStoreAsync(CacheSource, _entityFetcherMock.Object);

        result.Should().BeSameAs(user);
    }
    
    [Fact]
    public async Task RetrieveOrStoreAsync_RetrievedValueIsNull_CallsStoreStrategy()
    {
        var user = SetupEntityFetcherUser();
        
        var result = await _sut.RetrieveOrStoreAsync(CacheSource, _entityFetcherMock.Object);
        
        _storeStrategyMock
            .Verify(_ => _.StoreAsync(user), Times.Once);
    }

    private User SetupRetrievedUser()
    {
        var user = new User();
        
        _retrieveStrategyMock
            .Setup(_ => _.RetrieveAsync(CacheSource))
            .ReturnsAsync(user);

        return user;
    }
    
    private User SetupEntityFetcherUser()
    {
        var user = new User();
        _entityFetcherMock
            .Setup(f => f(CacheSource))
            .ReturnsAsync(user);

        return user;
    }
}