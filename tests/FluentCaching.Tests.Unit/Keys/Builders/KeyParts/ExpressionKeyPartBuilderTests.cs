using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders.KeyParts;
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
    public void Create_WhenCalled_CallsGetProperty()
    {
        MockExpressionRewrite();

        Create(_ => _.Name, _expressionsHelperMock.Object);

        _expressionsHelperMock
            .Verify(_ => _.GetPropertyName(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
    }
    
    [Fact]
    public void Create_WhenCalled_CallsRewriteWithSafeToString()
    {
        MockExpressionRewrite();

        Create(_ => _.Name, _expressionsHelperMock.Object);

        _expressionsHelperMock
            .Verify(_ => _.RewriteWithSafeToString(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
    }

    [Fact]
    public void Build_StoreContext_BuildsKeyPart()
    {
        var user = new User
        {
            Name = "user name"
        };
        MockExpressionRewrite();
        var builder = Create(_ => _.Name, _expressionsHelperMock.Object);

        var result = builder.Build(new KeyContext<User>(user));
        result.Should().Be(user.Name);
    }
    
    [Fact]
    public void Build_RetrieveContext_BuildsKeyPart()
    {
        var userName = "user name";
        var retrieveContext = new Dictionary<string, object>
        {
            { nameof(User.Name), userName }
        };
        _expressionsHelperMock
            .Setup(_ => _.GetPropertyName(It.IsAny<Expression<Func<User, string>>>()))
            .Returns(nameof(User.Name));
        MockExpressionRewrite();
        var builder = Create(_ => _.Name, _expressionsHelperMock.Object);

        var result = builder.Build(new KeyContext<User>(retrieveContext));
        result.Should().Be(userName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Build_ValueIsEmpty_ThrowsKeyPartMissingException(string missingName)
    {
        var user = new User
        {
            Name = missingName
        };
        MockExpressionRewrite();
        var builder = Create(_ => _.Name, _expressionsHelperMock.Object);
        
        builder.Invoking(_ => _.Build(new KeyContext<User>(user)))
            .Should()
            .Throw<KeyPartMissingException>();
    }
    
    private static ExpressionKeyPartBuilder<User> Create<T>(Expression<Func<User, T>> valueGetter, 
        IExpressionsHelper expressionsHelper) where T : class
        => ExpressionKeyPartBuilder<User>.Create(valueGetter, expressionsHelper);

    private void MockExpressionRewrite()
        => _expressionsHelperMock
            .Setup(_ => _.RewriteWithSafeToString<User, string>(
                It.IsAny<Expression<Func<User, string>>>()))
            .Returns(_ => _.Name == null ? null : _.Name.ToString());
}