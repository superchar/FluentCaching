using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentCaching.Keys.Builders;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.PolicyBuilders.Keys
{
    public class CombinedCachingKeyPolicyBuilderTests
    {
        private readonly Mock<IKeyBuilder> _keyBuilderMock;

        private readonly CombinedCachingKeyPolicyBuilder<User> _sut;

        public CombinedCachingKeyPolicyBuilderTests()
        {
            _keyBuilderMock = new Mock<IKeyBuilder>();

            _sut = new CombinedCachingKeyPolicyBuilder<User>(_keyBuilderMock.Object);
        }

        [Fact]
        public void And_HappyPath_ReturnsNotNullTtlTypeBuilder()
        {
            var result = _sut.And();

            result.Should().NotBeNull();
        }

        [Fact]
        public void CombinedWith_Expression_CallsTrackExpressionWithProperParameter()
        {
            var result = _sut.CombinedWith(u => u.Name);

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendExpression(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }

        [Fact]
        public void CombinedWith_StaticValue_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.CombinedWith("static value");

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendStatic("static value"), Times.Once);
        }

        [Fact]
        public void CombinedWithClassName_ClassName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.CombinedWithClassName();

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendStatic("User"), Times.Once);
        }

        [Fact]
        public void CombinedWithClassFullName_ClassNameFullName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.CombinedWithClassFullName();

            result.Should().NotBeNull();
            _keyBuilderMock
                .Verify(p => p.AppendStatic("FluentCaching.Tests.Unit.Models.User"), Times.Once);
        }
    }
}
