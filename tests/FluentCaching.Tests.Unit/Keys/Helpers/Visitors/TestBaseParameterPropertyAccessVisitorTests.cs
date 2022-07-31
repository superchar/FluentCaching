using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Helpers.Visitors;

public class TestBaseParameterPropertyAccessVisitorTests
{
    private readonly Mock<Action<MemberExpression>> _visitCallbackMock;

    private readonly TestBaseParameterPropertyAccessVisitor _sut;

    public TestBaseParameterPropertyAccessVisitorTests()
    {
        _visitCallbackMock = new Mock<Action<MemberExpression>>();

        _sut = new TestBaseParameterPropertyAccessVisitor(_visitCallbackMock.Object);
    }

    [Fact]
    public void GetPropertyMetadata_WhenCalled_ReturnsPropertyNameAndType()
    {
        Expression<Func<User, int>> expression = _ => _.Id;
        var member = expression.Body as MemberExpression;

        var (resultName, resultType) = TestBaseParameterPropertyAccessVisitor.GetPropertyMetadata(member);

        resultName.Should().Be(nameof(User.Id));
        resultType.Should().Be(typeof(int));
    }
    
    [Fact]
    public void Visit_SingleParameterPropertyExpression_InvokesCallbackForProperty()
    {
        Expression<Func<User, int>> expression = _ => _.Id;

        _sut.Visit(expression.Body);
        
        _visitCallbackMock
            .Verify(f => f(It.IsAny<MemberExpression>()), Times.Once);
        _visitCallbackMock
            .Verify(f => f(It.Is<MemberExpression>(
                _ => _.Member.Name == nameof(User.Id))), Times.Once);
    }
    
    [Fact]
    public void Visit_ClosurePropertyExpression_InvokesCallbackOnlyForParameterProperty()
    {
        var testCloseObj = new { Value = 42 };
        
        Expression<Func<User, int>> expression = _ => _.Id + testCloseObj.Value;

        _sut.Visit(expression.Body);
        
        _visitCallbackMock
            .Verify(f => f(It.IsAny<MemberExpression>()), Times.Once);
        _visitCallbackMock
            .Verify(f => f(It.Is<MemberExpression>(
                _ => _.Member.Name == nameof(User.Id))), Times.Once);
    }
    
    [Fact]
    public void Visit_MultipleParameterPropertyExpressions_InvokesCallbackForEachParameterProperty()
    {
        Expression<Func<User, int>> expression = _ => _.Id + _.SubscriptionId.Value;

        _sut.Visit(expression.Body);
        
        _visitCallbackMock
            .Verify(f => f(It.IsAny<MemberExpression>()), Times.Exactly(2));
        _visitCallbackMock
            .Verify(f => f(It.Is<MemberExpression>(
                _ => _.Member.Name == nameof(User.Id))), Times.Once);
        _visitCallbackMock
            .Verify(f => f(It.Is<MemberExpression>(
                _ => _.Member.Name == nameof(User.SubscriptionId))), Times.Once);
    }

    [Fact]
    public void Visit_NoParameterPropertyExpressions_DoesNotInvokeCallback()
    {
        Expression<Func<User, int>> expression = _ => 1 + 42;

        _sut.Visit(expression.Body);

        _visitCallbackMock
            .Verify(f => f(It.IsAny<MemberExpression>()), Times.Never);
    }
}