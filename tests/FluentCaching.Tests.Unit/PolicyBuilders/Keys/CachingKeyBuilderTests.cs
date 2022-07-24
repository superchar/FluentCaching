using System;
using System.Linq.Expressions;
using Moq;
using Xunit;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.Tests.Unit.Models;

namespace FluentCaching.Tests.Unit.PolicyBuilders.Keys
{
    public class CachingKeyBuilderTests
    {
        private readonly Mock<IKeyBuilder<User>> _keyBuilderMock;

        private readonly CachingKeyBuilder<User> _sut;

        public CachingKeyBuilderTests()
        {
            _keyBuilderMock = new Mock<IKeyBuilder<User>>();

            _sut = new CachingKeyBuilder<User>(_keyBuilderMock.Object);
        }

        [Fact]
        public void UseAsKey_Expression_CallsTrackExpressionWithProperParameter()
        {
            var result = _sut.UseAsKey(u => u.Name);

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendExpression(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }

        [Fact]
        public void UseAsKey_StaticValue_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.UseAsKey("static value");

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendStatic("static value"), Times.Once);
        }

        [Fact]
        public void UseClassNameAsKey_ClassName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.UseClassNameAsKey();

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendStatic("User"), Times.Once);
        }

        [Fact]
        public void UseClassFullNameAsKey_ClassNameFullName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.UseClassFullNameAsKey();

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendStatic("FluentCaching.Tests.Unit.Models.User"), Times.Once);
        }
    }
}
