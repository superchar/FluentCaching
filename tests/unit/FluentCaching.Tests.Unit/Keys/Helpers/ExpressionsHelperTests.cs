using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Helpers;

public class ExpressionsHelperTests
{
    private readonly ExpressionsHelper _sut = new ();

    [Fact]
    public void GetProperty_ExpressionWithoutProperties_ReturnsEmptyArray()
    {
        var result = _sut.GetParameterPropertyNames<User, string>(u => "test");

        result.Should().BeEmpty();
    }
    
    [Fact]
    public void GetProperty_ExpressionWithSingleNonNullableProperty_ReturnsPropertyName()
    {
        var result = _sut.GetParameterPropertyNames<User, int>(u => u.Id);

        var expectedProperties = new[] { nameof(User.Id) };
        result.Should().BeEquivalentTo(expectedProperties);
    }
    
    [Fact]
    public void GetProperty_ExpressionWithSingleNullableProperty_ReturnsPropertyName()
    {
        var result = _sut.GetParameterPropertyNames<User, int>(u => u.SubscriptionId.Value);

        var expectedProperties = new[] { nameof(User.SubscriptionId) };
        result.Should().BeEquivalentTo(expectedProperties);
    }

    [Fact]
    public void GetProperty_ExpressionWithMultipleProperties_ReturnsPropertyNames()
    {
        var result = _sut.GetParameterPropertyNames<User, int>(u => u.Id + u.SubscriptionId.Value);

        var expectedProperties = new[] { nameof(User.Id), nameof(User.SubscriptionId) };
        result.Should().BeEquivalentTo(expectedProperties);
    }

    [Fact]
    public void ReplaceResultTypeWithString_PropertyIsPrimitiveType_ReplacesResultTypeWithString()
    {
        var user = new User
        {
            Id = 42
        };

        var resultExpression = _sut.ReplaceResultTypeWithString<User, int>(_ => _.Id);

        var compiledExpression = resultExpression.Compile();
        var result = compiledExpression(user);
        result.Should().Be("42");
    }
        
    [Fact]
    public void ReplaceResultTypeWithString_PropertyIsNullablePrimitiveType_ReplacesResultTypeWithString()
    {
        var user = new User
        {
            SubscriptionId = 42
        };

        var resultExpression = _sut.ReplaceResultTypeWithString<User, int?>(_ => _.SubscriptionId.Value);

        var compiledExpression = resultExpression.Compile();
        var result = compiledExpression(user);
        result.Should().Be("42");
    }
        
    [Fact]
    public void ReplaceResultTypeWithString_PropertyIsReferenceType_ReplacesResultTypeWithString()
    {
        var user = new User
        {
            Currency = new Currency("USD")
        };

        var resultExpression = _sut.ReplaceResultTypeWithString<User, Currency>(_ => _.Currency);

        var compiledExpression = resultExpression.Compile();
        var result = compiledExpression(user);
        result.Should().Be("USD");
    }
    
    [Fact]
    public void ReplaceResultTypeWithString_PropertyIsStringType_DoesNotChangeExpression()
    {
        var user = new User
        {
            Name = "Some name"
        };
        Expression<Func<User, string>> expression = _ => _.Name;

        var resultExpression = _sut.ReplaceResultTypeWithString(expression);

        resultExpression.Body.Should().Be(expression.Body);
    }

    [Fact]
    public void ReplaceResultTypeWithString_NullablePrimitiveTypePropertyIsNull_ReturnsNull()
    {
        var resultExpression = _sut.ReplaceResultTypeWithString<User, int?>(_ => _.SubscriptionId);

        var compiledExpression = resultExpression.Compile();
        var result = compiledExpression(new User());
        result.Should().BeNull();
    }
        
    [Fact]
    public void ReplaceResultTypeWithString_ReferenceTypePropertyIsNull_ReturnsNull()
    {
        var resultExpression = _sut.ReplaceResultTypeWithString<User, Currency>(_ => _.Currency);

        var compiledExpression = resultExpression.Compile();
        var result = compiledExpression(new User());
        result.Should().BeNull();
    }
    
    [Fact]
    public void ReplaceResultTypeWithString_PropertyIsValueType_DoesNotAddNullCheck()
    {
        var resultExpression = _sut.ReplaceResultTypeWithString<User, int>(_ => _.Id);

        resultExpression.Body.NodeType.Should().Be(ExpressionType.Call);
    }
        
    [Fact]
    public void GetProperties_TypeHasNoProperties_ReturnsEmptyArray()
    {
        var result = _sut.GetProperties(typeof(TypeWithoutProperties));

        result.Should().BeEmpty();
    }

    [Fact]
    public void GetProperties_TypeHasProperties_ReturnsPropertiesArray()
    {
        var result = _sut.GetProperties(typeof(User));

        result.Should().HaveCount(4);
        result.Should().Contain(e => e.Name == nameof(User.Name))
            .And.Contain(e => e.Name == nameof(User.Id))
            .And.Contain(e => e.Name == nameof(User.Currency))
            .And.Contain(e => e.Name == nameof(User.SubscriptionId));
    }
}