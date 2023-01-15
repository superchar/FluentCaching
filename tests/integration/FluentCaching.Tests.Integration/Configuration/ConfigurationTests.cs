﻿using System;
using FluentAssertions;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;
using Xunit;

namespace FluentCaching.Tests.Integration.Configuration;

public class ConfigurationTests : BaseTest
{
    [Fact]
    public void ConstantExpressionConfiguration_DoesNotThrowException()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(_ => 1 + 1).Complete());

        CacheBuilder.Invoking(_ => _.Build()).Should().NotThrow();
    }
        
    [Fact]
    public void ClosureConfiguration_DoesNotThrowException()
    {
        var value = new Random().Next();
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.Id + value).Complete());
            
        CacheBuilder.Invoking(_ => _.Build()).Should().NotThrow();
    }
        
    [Fact]
    public void NestedStructPropertyConfiguration_DoesNotThrowException()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.Address.Street).Complete());
            
        CacheBuilder.Invoking(_ => _.Build()).Should().NotThrow();
    }
        
    [Fact]
    public void NestedClassPropertyConfiguration_DoesNotThrowException()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.LastOrder.OrderId).Complete());
            
        CacheBuilder.Invoking(_ => _.Build()).Should().NotThrow();
    }
}