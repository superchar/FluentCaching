using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders
{
    public class KeyBuilderTests
    {
        private readonly Mock<IKeyContextBuilder<User>> _keyContextBuilderMock;
        private readonly Mock<IExpressionsHelper> _expressionHelperMock;

        private KeyBuilder<User> _sut;

        public KeyBuilderTests()
        {
            _keyContextBuilderMock = new Mock<IKeyContextBuilder<User>>();
            _expressionHelperMock = new Mock<IExpressionsHelper>();

            _sut = new KeyBuilder<User>(_keyContextBuilderMock.Object, _expressionHelperMock.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AppendStatic_ValueIsEmpty_ThrowsKeyPartMissingException(string missingValue)
        {
            _sut.Invoking(_ => _.AppendStatic(missingValue))
                .Should()
                .Throw<KeyPartMissingException>();
        }

        [Fact]
        public void AppendStatic_WhenCalled_AddsValueToKeyParts()
        {
            _sut.AppendStatic("Value");

            var result = _sut.BuildFromStaticKey();
            result.Should().Be("Value");
        }

        [Fact]
        public void AppendExpression_WhenCalled_CallsGetProperty()
        {
            MockProperty();
            MockExpressionRewrite();

            _sut.AppendExpression(_ => _.Name);

            _expressionHelperMock
                .Verify(_ => _.GetProperty(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }

        [Fact]
        public void AppendExpression_WhenCalled_CallsRewriteWithSafeToString()
        {
            MockProperty();
            MockExpressionRewrite();

            _sut.AppendExpression(_ => _.Name);

            _expressionHelperMock
                .Verify(_ => _.RewriteWithSafeToString(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }

        [Fact]
        public void AppendExpression_WhenCalled_AddsKeyToContext()
        {
            MockProperty(nameof(User.Name));
            MockExpressionRewrite();

            _sut.AppendExpression(_ => _.Name);

            _keyContextBuilderMock
                .Verify(_ => _.AddKey(nameof(User.Name)), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AppendExpression_ValueIsEmpty_ThrowsKeyPartMissingException(string missingName)
        {
            var user = new User
            {
                Name = missingName
            };
            MockCacheContext(user);
            MockProperty();
            MockExpressionRewrite();

            _sut.AppendExpression(_ => _.Name);

            _sut.Invoking(_ => _.BuildFromCachedObject(user))
                .Should()
                .Throw<KeyPartMissingException>();
        }

        [Fact]
        public void AppendExpression_WhenCalled_AddsValueToKeyParts()
        {
            var user = new User
            {
                Name = "user name"
            };
            MockCacheContext(user);
            MockProperty();
            MockExpressionRewrite();
            _sut.AppendExpression(_ => _.Name);

            var result = _sut.BuildFromCachedObject(user);
            result.Should().Be("user name");
        }
        
        [Fact]
        public void BuildFromCachedObject_WhenCalled_CallsKeyContextBuilder()
        {
            var user = new User
            {
                Name = "John"
            };
            MockCacheContext(user);
            _sut.AppendStatic("user");

            var result = _sut.BuildFromCachedObject(user);
            
            _keyContextBuilderMock
                .Verify(_ => _.BuildCacheContext(user), Times.Once);
        }


        [Fact]
        public void BuildFromCachedObject_StaticAndExpressionKeyPartsExist_BuildsKey()
        {
            var user = new User
            {
                Name = "John"
            };
            MockCacheContext(user);
            MockProperty(nameof(User.Name));
            MockExpressionRewrite();
            _sut.AppendStatic("user");
            _sut.AppendExpression(_ => _.Name);

            var result = _sut.BuildFromCachedObject(user);

            result.Should().Be("userJohn");
        }

        [Fact]
        public void BuildFromStaticKey_ExpressionPartsExist_ThrowsKeyPartMissingException()
        {
            MockProperty();
            MockExpressionRewrite();
            _sut.AppendExpression(_ => _.Name);

            _sut.Invoking(_ => _.BuildFromStaticKey())
                .Should()
                .Throw<KeyPartMissingException>();
        }

        [Fact]
        public void BuildFromStaticKey_StaticKeyPartsExist_BuildsStaticKey()
        {
            _sut.AppendStatic("first");
            _sut.AppendStatic("second");

            var result = _sut.BuildFromStaticKey();

            result.Should().Be("firstsecond");
        }

        [Fact]
        public void BuildFromStringKey_WhenCalled_CallsKeyContextBuilder()
        {
            _sut.BuildFromStringKey("UserName");

            _keyContextBuilderMock
                .Verify(_ => _.BuildRetrieveContextFromStringKey("UserName"), Times.Once);
        }

        [Fact]
        public void BuildFromStringKey_StaticAndExpressionKeyPartsExist_BuildsKey()
        {
            MockProperty(nameof(User.Name));
            MockExpressionRewrite();
            var retrieveContext = new Dictionary<string, object>
            {
                { nameof(User.Name), "UserName" }
            };
            var keyContext = new KeyContext<User>(retrieveContext);
            _keyContextBuilderMock
                .Setup(_ => _.BuildRetrieveContextFromStringKey("UserName"))
                .Returns(keyContext);
            _sut.AppendStatic("key");
            _sut.AppendExpression(_ => _.Name);

            var result = _sut.BuildFromStringKey("UserName");

            result.Should().Be("keyUserName");
        }

        [Fact]
        public void BuildFromObjectKey_WhenCalled_CallsKeyContextBuilder()
        {
            var obj = new { };

            _sut.BuildFromObjectKey(obj);

            _keyContextBuilderMock
                .Verify(_ => _.BuildRetrieveContextFromObjectKey(obj), Times.Once);
        }

        [Fact]
        public void BuildFromObjectKey_StaticAndExpressionKeyPartsExist_BuildsKey()
        {
            MockProperty(nameof(User.Name));
            MockExpressionRewrite();
            var objectKey = new { Name = "UserName" };
            var retrieveContext = new Dictionary<string, object>
            {
                { nameof(User.Name), "UserName" }
            };
            var keyContext = new KeyContext<User>(retrieveContext);
            _keyContextBuilderMock
                .Setup(_ => _.BuildRetrieveContextFromObjectKey(objectKey))
                .Returns(keyContext);
            _sut.AppendStatic("key");
            _sut.AppendExpression(_ => _.Name);

            var result = _sut.BuildFromObjectKey(objectKey);

            result.Should().Be("keyUserName");
        }

        private void MockProperty(string name = default)
        {
            var memberInfoMock = new Mock<MemberInfo>();
            memberInfoMock
                .SetupGet(_ => _.Name)
                .Returns(name);
            _expressionHelperMock
                .Setup(_ => _.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(memberInfoMock.Object);
        }

        private void MockExpressionRewrite()
            => _expressionHelperMock
                .Setup(_ => _.RewriteWithSafeToString<User, string>(
                    It.IsAny<Expression<Func<User, string>>>()))
                .Returns(_ => _.Name == null ? null : _.Name.ToString());

        private void MockCacheContext(User user) =>
            _keyContextBuilderMock
                .Setup(_ => _.BuildCacheContext(user))
                .Returns(new KeyContext<User>(user));
    }
}