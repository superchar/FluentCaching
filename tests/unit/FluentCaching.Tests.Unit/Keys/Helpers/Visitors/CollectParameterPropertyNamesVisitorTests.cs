using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Keys.Helpers.Visitors;
using FluentCaching.Tests.Unit.TestModels;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Helpers.Visitors;

public class CollectParameterPropertyNamesVisitorTests
{
    private readonly CollectParameterPropertyNamesVisitor _sut;

    public CollectParameterPropertyNamesVisitorTests()
    {
        _sut = new CollectParameterPropertyNamesVisitor();
    }
    
    [Fact]
    public void Visit_SingleParameterPropertyExpression_CollectsParameterProperty()
    {
        Expression<Func<User, int>> expression = _ => _.Id;

        _sut.Visit(expression.Body);

        _sut.Properties.Should().OnlyContain(n => n.Equals(nameof(User.Id)));
    }
    
    [Fact]
    public void Visit_ClosurePropertyExpression_CollectsOnlyParameterProperty()
    {
        var testCloseObj = new { Value = 42 };
        Expression<Func<User, int>> expression = _ => _.Id + testCloseObj.Value;

        _sut.Visit(expression.Body);
        
        _sut.Properties.Should().OnlyContain(n => n.Equals(nameof(User.Id)));
    }
    
    [Fact]
    public void Visit_MultipleParameterPropertyExpressions_CollectsAllParameterProperties()
    {
        Expression<Func<User, int>> expression = _ => _.Id + _.SubscriptionId.Value;
        _sut.Visit(expression.Body);

        var expectedResult = new[] { nameof(User.Id), nameof(User.SubscriptionId) };
        _sut.Properties.Should().BeEquivalentTo(expectedResult);
    }
    
    [Fact]
    public void Visit_NoParameterPropertyExpressions_ReturnsEmptyList()
    {
        Expression<Func<User, int>> expression = _ => 2 + 2;
        _sut.Visit(expression.Body);

        _sut.Properties.Should().BeEmpty();
    }
}