using System.Collections.Generic;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Helpers;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders
{
    public class KeyContextBuilderTests
    {
        private Mock<IComplexKeysHelper> _complexKeysHelperMock;

        private KeyContextBuilder _sut;

        public KeyContextBuilderTests()
        {
            _complexKeysHelperMock = new Mock<IComplexKeysHelper>();

            _sut = new KeyContextBuilder(_complexKeysHelperMock.Object);
        }

        [Fact]
        public void AddKey_WhenCalled_IncludesKeyInContext()
        {
            const string key = "key";
            
            _sut.AddKey(key);

            var result = _sut.BuildKeyContextFromString("some string");
            result.Should().ContainKey(key);
        }
        
        [Fact]
        public void BuildKeyContextFromString_KeysCountAreGreaterThanOne_ThrowsKeyPartMissingException()
        {
            _sut.AddKey("first");
            _sut.AddKey("second");

            _sut.Invoking(_ => _.BuildKeyContextFromString("target string"))
                .Should()
                .Throw<KeyPartMissingException>();
        }
        
        [Fact]
        public void BuildKeyContextFromString_NoKeysAdded_ReturnsEmptyContext()
        {
            var result = _sut.BuildKeyContextFromString("target string");

            result.Should().BeEmpty();
        }
        
        [Fact]
        public void BuildKeyContextFromString_SingleKeyAdded_ReturnsSingleKeyContext()
        {
            const string key = "key";
            const string targetString = "target string";
            _sut.AddKey(key);
            
            var result = _sut.BuildKeyContextFromString(targetString);

            result.Keys.Should().HaveCount(1);
            result.Should().ContainKey(key).WhoseValue.Should().Be(targetString);
        }
        
        [Fact]
        public void BuildKeyContextFromObject_CallsComplexKeysHelper_WhenCalled()
        {
            _sut.BuildKeyContextFromObject(new object());
            
            _complexKeysHelperMock
                .Verify(_ => _.GetProperties(typeof(object)), Times.Once);
        }

        [Fact]
        public void BuildKeyContextFromObject_PropertyConfiguredAsKeyIsMissing_ThrowsKeyPartMissingException()
        {
            var objectKey = new { FirstKey = "Value" };
            _sut.AddKey(nameof(objectKey.FirstKey));
            _sut.AddKey("SecondKey");

            _sut.Invoking(_ => _.BuildKeyContextFromObject(objectKey))
                .Should()
                .Throw<KeyPartMissingException>();
        }
        
        [Fact]
        public void BuildKeyContextFromObject_PropertyIsNotConfiguredAsKey_SkipsSuchProperty()
        {
            var objectKey = new { FirstKey = "Value", SecondKey = "Second Value" };
            _sut.AddKey(nameof(objectKey.FirstKey));
            var properties = new[]
            {
                new PropertyAccessor(nameof(objectKey.FirstKey), _ => _),
                new PropertyAccessor(nameof(objectKey.SecondKey), _ => _)
            };
            _complexKeysHelperMock
                .Setup(_ => _.GetProperties(objectKey.GetType()))
                .Returns(properties);

            var result = _sut.BuildKeyContextFromObject(objectKey);

            result.Should().HaveCount(1);
            result.Should().ContainKey(nameof(objectKey.FirstKey));
        }
        
        [Fact]
        public void BuildKeyContextFromObject_DefaultHappyPath_ReturnsContext()
        {
            var objectKey = new { FirstKey = "Value", SecondKey = "Second Value" };
            _sut.AddKey(nameof(objectKey.FirstKey));
            _sut.AddKey(nameof(objectKey.SecondKey));
            var properties = new[]
            {
                new PropertyAccessor(nameof(objectKey.FirstKey), _ => _),
                new PropertyAccessor(nameof(objectKey.SecondKey), _ => _)
            };
            _complexKeysHelperMock
                .Setup(_ => _.GetProperties(objectKey.GetType()))
                .Returns(properties);

            var result = _sut.BuildKeyContextFromObject(objectKey);

            result.Should().HaveCount(2);
            result.Should().ContainKey(nameof(objectKey.FirstKey));
            result.Should().ContainKey(nameof(objectKey.SecondKey));
        }
    }
}