using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Builders.KeyParts.Factories;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders
{
    public class KeyBuilderTests
    {
        private readonly Mock<IKeyContextBuilder> _keyContextBuilderMock;
        private readonly Mock<IExpressionsHelper> _expressionHelperMock;
        private readonly Mock<IKeyPartBuilderFactory> _keyPartBuilderFactoryMock;

        private KeyBuilder _sut;

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

            _sut.AppendStatic(keyPart);

            _keyPartBuilderFactoryMock
                .Verify(_ => _.Create(keyPart), Times.Once);
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
            _keyContextBuilderMock
                .Setup(_ => _.BuildCacheContext(user))
                .Returns(new KeyContext(user));
            var keyPartBuilderMock = new Mock<IKeyPartBuilder>();
            _keyPartBuilderFactoryMock
                .Setup(_ => _.Create("user"))
                .Returns(keyPartBuilderMock.Object);
            _sut.AppendStatic("user");

            var result = _sut.BuildFromCachedObject(user);

            _keyContextBuilderMock
                .Verify(_ => _.BuildCacheContext(user), Times.Once);
        }

        [Fact]
        public void BuildFromStaticKey_DynamicPartsExist_ThrowsKeyPartMissingException()
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

            _sut.Invoking(_ => _.BuildFromStaticKey())
                .Should()
                .Throw<KeyPartMissingException>();
        }

        [Fact]
        public void BuildFromStringKey_WhenCalled_CallsKeyContextBuilder()
        {
            _sut.BuildFromStringKey("UserName");

            _keyContextBuilderMock
                .Verify(_ => _.BuildRetrieveContextFromStringKey("UserName"), Times.Once);
        }

        [Fact]
        public void BuildFromObjectKey_WhenCalled_CallsKeyContextBuilder()
        {
            var obj = new { };

            _sut.BuildFromObjectKey(obj);

            _keyContextBuilderMock
                .Verify(_ => _.BuildRetrieveContextFromObjectKey(obj), Times.Once);
        }
        
        private void MockProperties(params string [] properties) =>
            _expressionHelperMock
                .Setup(_ => _.GetParameterPropertyNames(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(properties);
    }
}