using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders.KeyParts;

public class ExpressionKeyPartBuilderTests
{
    private readonly Mock<IExpressionsHelper> _expressionsHelperMock;

    public ExpressionKeyPartBuilderTests()
    {
        _expressionsHelperMock = new Mock<IExpressionsHelper>();
    }

    [Fact]
    public void Create_WhenCalled_CallsReplaceResultTypeWithString()
    {
        SetupExpressionRewriteFakes();

        Create(_ => _.SubscriptionId, _expressionsHelperMock.Object);

        _expressionsHelperMock
            .Verify(_ => _.ReplaceResultTypeWithString(It.IsAny<Expression<Func<User, int?>>>()), Times.Once);
    }

    [Fact]
    public void Create_WhenCalled_CallsReplaceParameterWithDictionary()
    {
        SetupExpressionRewriteFakes();

        Create(_ => _.SubscriptionId, _expressionsHelperMock.Object);

        _expressionsHelperMock
            .Verify(_ => _.ReplaceParameterWithDictionary(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
    }

    [Fact]
    public void Build_StoreContext_BuildsKeyPart()
    {
        var user = new User
        {
            SubscriptionId = 42
        };
        SetupExpressionRewriteFakes();
        var builder = Create(_ => _.SubscriptionId, _expressionsHelperMock.Object);

        var result = builder.Build(new KeyContext(user));
        result.Should().Be(user.SubscriptionId.ToString());
    }

    [Fact]
    public void Build_RetrieveContext_BuildsKeyPart()
    {
        const int subscriptionId = 42;
        var retrieveContext = new Dictionary<string, object>
        {
            { nameof(User.SubscriptionId), subscriptionId }
        };
        SetupExpressionRewriteFakes();
        var builder = Create(_ => _.SubscriptionId, _expressionsHelperMock.Object);

        var result = builder.Build(new KeyContext(retrieveContext));
        result.Should().Be(subscriptionId.ToString());
    }

    [Fact]
    public void Build_ValueIsNull_ThrowsKeyPartMissingException()
    {
        var user = new User
        {
            SubscriptionId = null
        };
        SetupExpressionRewriteFakes();
        var builder = Create(_ => _.SubscriptionId, _expressionsHelperMock.Object);

        builder.Invoking(_ => _.Build(new KeyContext(user)))
            .Should()
            .Throw<KeyPartMissingException>();
    }
    
    [Fact]
    public void IsDynamic_WhenCalled_ReturnsTrue()
    {
        SetupExpressionRewriteFakes();
        var builder = Create(_ => _.SubscriptionId, _expressionsHelperMock.Object);

        builder.IsDynamic.Should().BeTrue();
    }

    private static ExpressionKeyPartBuilder Create<T>(Expression<Func<User, T>> valueGetter,
        IExpressionsHelper expressionsHelper)
        => ExpressionKeyPartBuilder.Create(valueGetter, expressionsHelper);

    private void SetupExpressionRewriteFakes()
    {
        _expressionsHelperMock
            .Setup(_ => _.ReplaceResultTypeWithString(It.IsAny<Expression<Func<User, int?>>>()))
            .Returns(_ => _.SubscriptionId == null ? null : _.SubscriptionId.ToString());

        _expressionsHelperMock
            .Setup(_ => _.ReplaceParameterWithDictionary(
                It.IsAny<Expression<Func<User, string>>>()))
            .Returns(_ => _[nameof(User.SubscriptionId)] == null ? null : _[nameof(User.SubscriptionId)].ToString());
    }
}