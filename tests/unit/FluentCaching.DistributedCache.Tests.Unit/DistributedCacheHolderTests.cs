using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using static FluentCaching.DistributedCache.Tests.Unit.MockHelper;

namespace FluentCaching.DistributedCache.Tests.Unit;

public class DistributedCacheHolderTests : IDisposable
{
    private readonly Mock<IDistributedCache> _distributedCacheMock;
    private Mock<IServiceProvider> _scopeServiceProviderMock;
    private Mock<IServiceScope> _serviceScopeMock;

    public DistributedCacheHolderTests()
    {
        _distributedCacheMock = new Mock<IDistributedCache>(); 
        MockScopeAndServiceProvider();
    }

    [Fact]
    public void Ctor_CacheParameterIsProvided_InitializedDistributedCacheFromParameter()
    {
        var holder = new DistributedCacheHolder(_distributedCacheMock.Object);

        holder.Cache.Should().NotBeNull();
        _scopeServiceProviderMock.Verify(p => p.GetService(typeof(IDistributedCache)), Times.Never);
    }
        
    [Fact]
    public void Ctor_CacheParameterIsProvided_InitializedDistributedCacheFromScopeProvider()
    {
        var holder = new DistributedCacheHolder();

        holder.Cache.Should().NotBeNull();
        _scopeServiceProviderMock.Verify(p => p.GetService(typeof(IDistributedCache)), Times.Once);
    }
        
    [Fact]
    public void Dispose_CacheParameterIsProvided_DoesNotDisposeScope()
    {
        var holder = new DistributedCacheHolder(_distributedCacheMock.Object);
            
        holder.Dispose();

        _serviceScopeMock.Verify(s => s.Dispose(), Times.Never);
    }
        
    [Fact]
    public void Dispose_CacheParameterIsNotProvided_DisposesScope()
    {
        var holder = new DistributedCacheHolder();
            
        holder.Dispose();

        _serviceScopeMock.Verify(s => s.Dispose(), Times.Once);
    }

    private void MockScopeAndServiceProvider()
    {
        var (serviceScopeMock, scopeServiceProviderMock) = MockServiceLocator();
        _serviceScopeMock = serviceScopeMock;
        _scopeServiceProviderMock = scopeServiceProviderMock;
        _scopeServiceProviderMock
            .Setup(s => s.GetService(typeof(IDistributedCache)))
            .Returns(_distributedCacheMock.Object);
    }
    
    void IDisposable.Dispose()
        => ServiceLocator.Initialize(null);
}