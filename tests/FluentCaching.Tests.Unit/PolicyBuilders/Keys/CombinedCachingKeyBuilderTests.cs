using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.Tests.Unit.Models;
using Moq;
using System;
using System.Linq.Expressions;
using Xunit;

namespace FluentCaching.Tests.Unit.PolicyBuilders.Keys
{
    public class CombinedCachingKeyBuilderTests
    {
        private readonly Mock<IPropertyTracker<User>> _propertyTrackerMock;

        private readonly CombinedCachingKeyBuilder<User> _sut;

        public CombinedCachingKeyBuilderTests()
        {
            _propertyTrackerMock = new Mock<IPropertyTracker<User>>();

            _sut = new CombinedCachingKeyBuilder<User>(_propertyTrackerMock.Object);
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
            _propertyTrackerMock
                .Verify(p => p.TrackExpression(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }

        [Fact]
        public void CombinedWith_StaticValue_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.CombinedWith("static value");

            result.Should().NotBeNull();
            _propertyTrackerMock
                .Verify(p => p.TrackStatic("static value"), Times.Once);
        }

        [Fact]
        public void CombinedWithClassName_ClassName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.CombinedWithClassName();

            result.Should().NotBeNull();
            _propertyTrackerMock
                .Verify(p => p.TrackStatic("User"), Times.Once);
        }

        [Fact]
        public void CombinedWithClassFullName_ClassNameFullName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.CombinedWithClassFullName();

            result.Should().NotBeNull();
            _propertyTrackerMock
                .Verify(p => p.TrackStatic("FluentCaching.Tests.Unit.Models.User"), Times.Once);
        }
    }
}
