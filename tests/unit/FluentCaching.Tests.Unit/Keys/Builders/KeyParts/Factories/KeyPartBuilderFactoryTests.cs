using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Builders.KeyParts.Factories;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders.KeyParts.Factories;

public class KeyPartBuilderFactoryTests
{
    private readonly Mock<IExpressionsHelper> _expressionsHelperMock;

    private readonly KeyPartBuilderFactory _sut;

    public KeyPartBuilderFactoryTests()
    {
        _expressionsHelperMock = new Mock<IExpressionsHelper>();

        _sut = new KeyPartBuilderFactory(_expressionsHelperMock.Object);
    }

    [Fact]
    public void Create_StaticValue_ReturnsStaticKeyPartBuilder()
    {
        var result = _sut.Create<User, string>("test");

        result.Should().BeOfType<StaticKeyPartBuilder<User>>();
    }
    
    [Fact]
    public void Create_ExpressionValue_ReturnsExpressionKeyPartBuilder()
    {
        _expressionsHelperMock
            .Setup(_ => _.ReplaceResultTypeWithString(It.IsAny<Expression<Func<User, string>>>()))
            .Returns(_ => _.Name.ToString());
        _expressionsHelperMock
            .Setup(_ => _.ReplaceParameterWithDictionary(It.IsAny<Expression<Func<User, string>>>()))
            .Returns(_ => _[nameof(User.Name)].ToString());

        var result = _sut.Create<User, string>(_ => _.Name);

        result.Should().BeOfType<ExpressionKeyPartBuilder<User>>();
    }
}