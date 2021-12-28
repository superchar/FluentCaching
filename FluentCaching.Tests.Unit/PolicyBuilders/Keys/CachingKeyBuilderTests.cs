using System;
using System.Linq.Expressions;
using Moq;
using Xunit;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.PolicyBuilders.Keys;
using FluentCaching.Tests.Unit.Models;

namespace FluentCaching.Tests.Unit.PolicyBuilders.Keys
{
    public class CachingKeyBuilderTests
    {
        private readonly Mock<IPropertyTracker<User>> _propertyTrackerMock;

        private readonly CachingKeyBuilder<User> _sut;

        public CachingKeyBuilderTests()
        {
            _propertyTrackerMock = new Mock<IPropertyTracker<User>>();

            _sut = new CachingKeyBuilder<User>(_propertyTrackerMock.Object);
        }

        [Fact]
        public void UseAsKey_Expression_CallsTrackExpressionWithProperParameter()
        {
            var result = _sut.UseAsKey(u => u.Name);

            result.Should().NotBeNull();
            _propertyTrackerMock
                .Verify(p => p.TrackExpression(It.IsAny<Expression<Func<User, string>>>()), Times.Once);
        }

        [Fact]
        public void UseAsKey_StaticValue_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.UseAsKey("static value");

            result.Should().NotBeNull();
            _propertyTrackerMock
                .Verify(p => p.TrackStatic("static value"), Times.Once);
        }

        [Fact]
        public void UseAsKey_ClassName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.UseClassNameAsKey();

            result.Should().NotBeNull();
            _propertyTrackerMock
                .Verify(p => p.TrackStatic("User"), Times.Once);
        }

        [Fact]
        public void UseAsKey_ClassNameFullName_CallsTrackStaticWithProperParameter()
        {
            var result = _sut.UseClassFullNameAsKey();

            result.Should().NotBeNull();
            _propertyTrackerMock
                .Verify(p => p.TrackStatic("FluentCaching.Tests.Unit.Models.User"), Times.Once);
        }
    }
}
