﻿using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace FluentCaching.DependencyInjectionExtensions.Tests.Unit;

public class ServiceCollectionExtensionsTests
{
    private readonly Mock<IServiceCollection> _serviceCollectionMock = new();
    private readonly Mock<Action<CacheBuilder>> _builderActionMock = new();

    [Fact]
    public void AddFluentCaching_WhenCalled_InvokesBuilderAction()
    {
        _serviceCollectionMock.Object.AddFluentCaching(_builderActionMock.Object);

        _builderActionMock.Verify(a => a(It.IsNotNull<CacheBuilder>()));
    }
        
    [Fact]
    public void AddFluentCaching_ActionCallbackIsSuccessful_AddsCacheToContainer()
    {
        _serviceCollectionMock.Object.AddFluentCaching(_builderActionMock.Object);

        _serviceCollectionMock.Verify(s => s.Add(It.Is<ServiceDescriptor>(d => d.Lifetime == ServiceLifetime.Singleton 
                                                                               && typeof(ICache) == d.ServiceType)), Times.Once);
    }
}