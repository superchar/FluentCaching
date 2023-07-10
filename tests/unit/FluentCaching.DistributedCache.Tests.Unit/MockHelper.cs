using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FluentCaching.DistributedCache.Tests.Unit;

public static class MockHelper
{
    public static (Mock<IServiceScope>, Mock<IServiceProvider>) MockServiceLocator()
    {
        var serviceScopeMock = new Mock<IServiceScope>();
        var scopeServiceProviderMock = new Mock<IServiceProvider>();
        serviceScopeMock
            .SetupGet(s => s.ServiceProvider)
            .Returns(scopeServiceProviderMock.Object);
            
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock
            .Setup(f => f.CreateScope())
            .Returns(serviceScopeMock.Object);

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(s => s.GetService(typeof(IServiceScopeFactory)))
            .Returns(serviceScopeFactoryMock.Object);
            
        ServiceLocator.Initialize(serviceProviderMock.Object);

        return (serviceScopeMock, scopeServiceProviderMock);
    }
}