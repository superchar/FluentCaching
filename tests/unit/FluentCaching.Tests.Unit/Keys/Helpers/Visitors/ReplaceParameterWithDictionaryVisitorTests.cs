using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Keys.Helpers.Visitors;
using FluentCaching.Tests.Unit.Models;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Helpers.Visitors;

public class ReplaceParameterWithDictionaryVisitorTests
{
    private const int UserId = 42;

    private static readonly ParameterExpression DictionaryParameter = 
        Expression.Parameter(typeof(Dictionary<string, object>));

    private readonly ReplaceParameterWithDictionaryVisitor _sut = new(DictionaryParameter);

    
    [Fact]
    public void Visit_SingleParameterPropertyExpression_ReplacesParameterProperty()
    {
        Expression<Func<User, string>> expression = _ => _.Id.ToString();

        var newBody = _sut.Visit(expression.Body);
        
        var dictionary = new Dictionary<string, object>
        {
            { nameof(User.Id), UserId }
        };
        var compiledExpression = BuildAndCompileExpression(newBody);
        compiledExpression(dictionary).Should().Be(UserId.ToString());
    }
    
    [Fact]
    public void Visit_ClosurePropertyExpression_ReplacesOnlyParameterProperty()
    {
        var testCloseObj = new { Value = 42 };
        Expression<Func<User, string>> expression = _ => (_.Id + testCloseObj.Value).ToString();

        var newBody = _sut.Visit(expression.Body);
        
        var dictionary = new Dictionary<string, object>
        {
            { nameof(User.Id), UserId }
        };
        var compiledExpression = BuildAndCompileExpression(newBody);
        var expectedResult = (UserId + testCloseObj.Value).ToString();
        compiledExpression(dictionary).Should().Be(expectedResult);
    }
    
    [Fact]
    public void Visit_MultipleParameterPropertyExpressions_ReplacesAllParameterProperties()
    {
        Expression<Func<User, string>> expression = _ => (_.Id + _.SubscriptionId.Value).ToString();
        
        var newBody = _sut.Visit(expression.Body);

        const int subscriptionId = 50;
        var dictionary = new Dictionary<string, object>
        {
            { nameof(User.Id), UserId },
            { nameof(User.SubscriptionId), subscriptionId }
        };
        var compiledExpression = BuildAndCompileExpression(newBody);
        var expectedResult = (UserId + subscriptionId).ToString();
        compiledExpression(dictionary).Should().Be(expectedResult);
    }
    
    [Fact]
    public void Visit_NoParameterPropertyExpressions_DoesNotReplaceAnything()
    {
        const int firstValue = 1;
        const int secondValue = 5;
        Expression<Func<User, string>> expression = _ => (firstValue + secondValue).ToString();

        _sut.Visit(expression.Body);

        var newBody = _sut.Visit(expression.Body);
        
        var compiledExpression = BuildAndCompileExpression(newBody);
        var expectedResult = (firstValue + secondValue).ToString();
        compiledExpression(new Dictionary<string, object>()).Should().Be(expectedResult);
    }

    private static Func<Dictionary<string, object>, string> BuildAndCompileExpression(Expression newBody)
        => Expression.Lambda<Func<Dictionary<string, object>, string>>(newBody, DictionaryParameter)
            .Compile();
}