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
        private readonly Mock<IKeyContextBuilder> _keyContextBuilderMock;
        private readonly Mock<IExpressionsHelper> _expressionHelperMock;

        private KeyBuilder<User> _sut;

        public KeyBuilderTests()
        {
            _keyContextBuilderMock = new Mock<IKeyContextBuilder>();
            _expressionHelperMock = new Mock<IExpressionsHelper>();

            _expressionHelperMock
                .Setup(_ => _.RewriteWithSafeToString<User, string>(
                    It.IsAny<Expression<Func<User, string>>>()))
                .Returns(_ => _.Name == null ? null : _.Name.ToString());

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
            
            _sut.AppendExpression(_ => _.Name);
            
            _expressionHelperMock
                .Verify(_ => _.GetProperty(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }
        
        [Fact]
        public void AppendExpression_WhenCalled_CallsRewriteWithSafeToString()
        {
            MockProperty();
            
            _sut.AppendExpression(_ => _.Name);
            
            _expressionHelperMock
                .Verify(_ => _.RewriteWithSafeToString(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }
        
        [Fact]
        public void AppendExpression_WhenCalled_AddsKeyToContext()
        {
            MockProperty(nameof(User.Name));

            _sut.AppendExpression(_ => _.Name);
            
            _keyContextBuilderMock
                .Verify(_ => _.AddKey(nameof(User.Name)), Times.Once);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void AppendExpression_ValueIsEmpty_ThrowsKeyPartMissingException(string missingName)
        {
            MockProperty();
            var user = new User
            {
                Name = missingName
            };
            
            _sut.AppendExpression(_ => _.Name);
            
            _sut.Invoking(_ => _.BuildFromCachedObject(user))
                .Should()
                .Throw<KeyPartMissingException>();
        }
        
        [Fact]
        public void AppendExpression_WhenCalled_AddsValueToKeyParts()
        {
            MockProperty();
            var user = new User
            {
                Name = "user name"
            };
            _sut.AppendExpression(_ => _.Name);

            var result = _sut.BuildFromCachedObject(user);
            result.Should().Be("user name");
        }
        
        [Fact]
        public void BuildFromCachedObject_StaticAndExpressionKeyPartsExist_BuildsKey()
        {
            MockProperty(nameof(User.Name));
            _sut.AppendStatic("user");
            _sut.AppendExpression(_ => _.Name);
            var user = new User
            {
                Name = "John"
            };

            var result = _sut.BuildFromCachedObject(user);

            result.Should().Be("userJohn");
        }
        
        [Fact]
        public void BuildFromStaticKey_ExpressionPartsExist_ThrowsKeyPartMissingException()
        {
            MockProperty();
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
                .Verify(_ => _.BuildKeyContextFromString("UserName"), Times.Once);
        }

        [Fact]
        public void BuildFromStringKey_StaticAndExpressionKeyPartsExist_BuildsKey()
        {
            MockProperty(nameof(User.Name));
            var keyContext = new Dictionary<string, object>
            {
                { nameof(User.Name), "UserName" }
            };
            _keyContextBuilderMock
                .Setup(_ => _.BuildKeyContextFromString("UserName"))
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
                .Verify(_ => _.BuildKeyContextFromObject(obj), Times.Once);
        }

        [Fact]
        public void BuildFromObjectKey_StaticAndExpressionKeyPartsExist_BuildsKey()
        {
            MockProperty(nameof(User.Name));
            var objectKey = new { Name = "UserName" };
            var keyContext = new Dictionary<string, object>
            {
                { nameof(User.Name), "UserName" }
            };
            _keyContextBuilderMock
                .Setup(_ => _.BuildKeyContextFromObject(objectKey))
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
    }
}