using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace FluentCaching.DistributedCache.Tests.Unit
{
    public class DistributedCacheHolderTests
    {
        private readonly Mock<IDistributedCache> _distributedCacheMock;
        private Mock<IServiceProvider> _scopeServiceProviderMock;
        private Mock<IServiceScope> _serviceScopeMock;

        public DistributedCacheHolderTests()
        {
            _distributedCacheMock = new Mock<IDistributedCache>(); 
            MockScopeServiceProvider();
        }

        [Fact]
        public void Ctor_CacheParameterIsProvided_InitializedDistributedCacheFromParameter()
        {
            var holder = new DistributedCacheHolder(_distributedCacheMock.Object);

            holder.DistributedCache.Should().NotBeNull();
            _scopeServiceProviderMock.Verify(p => p.GetService(typeof(IDistributedCache)), Times.Never);
        }
        
        [Fact]
        public void Ctor_CacheParameterIsProvided_InitializedDistributedCacheFromScopeProvider()
        {
            var holder = new DistributedCacheHolder();

            holder.DistributedCache.Should().NotBeNull();
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

        private void MockScopeServiceProvider()
        {
            _serviceScopeMock = new Mock<IServiceScope>();
            _scopeServiceProviderMock = new Mock<IServiceProvider>();
            _scopeServiceProviderMock
                .Setup(s => s.GetService(typeof(IDistributedCache)))
                .Returns(_distributedCacheMock.Object);
            _serviceScopeMock
                .SetupGet(s => s.ServiceProvider)
                .Returns(_scopeServiceProviderMock.Object);
            
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            serviceScopeFactoryMock
                .Setup(f => f.CreateScope())
                .Returns(_serviceScopeMock.Object);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactoryMock.Object);
            
            ServiceLocator.Initialize(serviceProviderMock.Object);
        }
    }
}