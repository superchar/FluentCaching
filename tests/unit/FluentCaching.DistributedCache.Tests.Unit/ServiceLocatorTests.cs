using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace FluentCaching.DistributedCache.Tests.Unit;

public class ServiceLocatorTests : IDisposable
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;

    public ServiceLocatorTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
    }

    [Fact]
    public void Initialize_WhenCalled_DoesNotThrowException()
    {
        var initialize = () => ServiceLocator.Initialize(_serviceProviderMock.Object);

        initialize.Should().NotThrow();
    }

    [Fact]
    public void CreateScope_InvokesServiceProvider_WhenServiceProviderInitialized()
    {
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock
            .Setup(f => f.CreateScope())
            .Returns(new Mock<IServiceScope>().Object);
        _serviceProviderMock
            .Setup(s => s.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);
        ServiceLocator.Initialize(_serviceProviderMock.Object);

        ServiceLocator.CreateScope();

        _serviceProviderMock.Verify(s => s.GetService(typeof(IServiceScopeFactory)), Times.Once);
    }
        
    [Fact]
    public void CreateScope_ThrowsServiceProviderNotInitializedException_WhenServiceProviderIsNotInitialized()
    {
        Action createScope = () => ServiceLocator.CreateScope();

        createScope.Should().Throw<ServiceProviderNotInitializedException>();
    }

    void IDisposable.Dispose()
        => ServiceLocator.Initialize(null);
}