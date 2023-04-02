using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Extensions;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Builders.KeyParts.Factories;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;
using FluentCaching.Tests.Unit.TestModels;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders
{
    public class KeyBuilderTests
    {
        private readonly Mock<IKeyContextBuilder> _keyContextBuilderMock;
        private readonly Mock<IExpressionsHelper> _expressionHelperMock;
        private readonly Mock<IKeyPartBuilderFactory> _keyPartBuilderFactoryMock;

        private readonly KeyBuilder _sut;

        public KeyBuilderTests()
        {
            _keyContextBuilderMock = new Mock<IKeyContextBuilder>();
            _expressionHelperMock = new Mock<IExpressionsHelper>();
            _keyPartBuilderFactoryMock = new Mock<IKeyPartBuilderFactory>();

            _sut = new KeyBuilder(_keyContextBuilderMock.Object,
                _expressionHelperMock.Object,
                _keyPartBuilderFactoryMock.Object);
        }

        [Fact]
        public void AppendStatic_WhenCalled_CallsKeyPartBuilderFactory()
        {
            const string keyPart = "key part";

            _sut.AppendStatic<User, string>(keyPart);

            _keyPartBuilderFactoryMock
                .Verify(_ => _.Create<User, string>(keyPart), Times.Once);
        }

        [Fact]
        public void AppendExpression_WhenCalled_CallsGetPropertyNames()
        {
            MockProperties();

            _sut.AppendExpression<User, string>(_ => _.Name);

            _expressionHelperMock
                .Verify(_ => _.GetParameterPropertyNames(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }

        [Fact]
        public void AppendExpression_WhenCalled_AddsKeysToContext()
        {
            var nameProperty = nameof(User.Name);
            var idProperty = nameof(User.Id);
            MockProperties(nameProperty, idProperty);

            _sut.AppendExpression<User, string>(_ => _.Name);

            _keyContextBuilderMock
                .Verify(_ => _.AddKey(nameProperty), Times.Once);
            _keyContextBuilderMock
                .Verify(_ => _.AddKey(idProperty), Times.Once);
        }

        [Fact]
        public void BuildFromCachedObject_WhenCalled_CallsKeyContextBuilder()
        {
            var user = new User
            {
                Name = "John"
            };
            var context = new KeyContext(user);
            _keyContextBuilderMock
                .Setup(_ => _.BuildCacheContext(user))
                .Returns(context);
            var keyPartBuilderMock = new Mock<IKeyPartBuilder>();
            keyPartBuilderMock
                .Setup(k => k.Build(context))
                .Returns("Key part");
            _keyPartBuilderFactoryMock
                .Setup(_ => _.Create<User, string>("user"))
                .Returns(keyPartBuilderMock.Object);
            _sut.AppendStatic<User, string>("user");

            _sut.BuildFromCachedObject(user);

            _keyContextBuilderMock
                .Verify(_ => _.BuildCacheContext(user), Times.Once);
        }

        [Fact]
        public void BuildFromStaticKey_DynamicPartsExist_ThrowsKeyHasDynamicPartsException()
        {
            MockProperties();
            var keyPartBuilderMock = new Mock<IKeyPartBuilder>();
            keyPartBuilderMock
                .SetupGet(_ => _.IsDynamic)
                .Returns(true);
            _keyPartBuilderFactoryMock
                .Setup(_ => _.Create(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(keyPartBuilderMock.Object);
            _sut.AppendExpression<User, string>(_ => _.Name);

            var expectedMessage =
                $"Cannot build key without parameters, because key cache configuration for {typeof(User).ToFullNameString()} contains dynamic parts." +
                " Please provide necessary parameters when performing cache operations.";
            _sut.Invoking(_ => _.BuildFromStaticKey<User>())
                .Should()
                .Throw<KeyHasDynamicPartsException>()
                .WithMessage(expectedMessage);
        }

        [Fact]
        public void BuildFromStaticKey_DynamicPartsDontExist_CallsKeyPartBuilder()
        {
            const string key = "key";
            MockProperties();
            var keyPartBuilderMock = new Mock<IKeyPartBuilder>();
            keyPartBuilderMock
                .SetupGet(_ => _.IsDynamic)
                .Returns(false);
            _keyPartBuilderFactoryMock
                .Setup(_ => _.Create<User, string>(key))
                .Returns(keyPartBuilderMock.Object);
            _sut.AppendStatic<User, string>(key);

            _sut.BuildFromStaticKey<User>();

            keyPartBuilderMock.Verify(_ => _.Build(KeyContext.Null), Times.Once);
        }

        [Fact]
        public void BuildFromScalarKey_WhenCalled_CallsKeyContextBuilder()
        {
            _sut.BuildFromScalarKey("UserName");

            _keyContextBuilderMock
                .Verify(_ => _.BuildRetrieveContextFromScalarKey("UserName"), Times.Once);
        }

        [Fact]
        public void BuildFromComplexKey_WhenCalled_CallsKeyContextBuilder()
        {
            var obj = new { };

            _sut.BuildFromComplexKey(obj);

            _keyContextBuilderMock
                .Verify(_ => _.BuildRetrieveContextFromComplexKey(obj), Times.Once);
        }

        private void MockProperties(params string[] properties) =>
            _expressionHelperMock
                .Setup(_ => _.GetParameterPropertyNames(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(properties);
    }
}