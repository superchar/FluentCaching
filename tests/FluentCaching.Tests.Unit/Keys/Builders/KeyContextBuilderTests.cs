using System.Linq;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders
{
    public class KeyContextBuilderTests
    {
        private Mock<IComplexKeysHelper> _complexKeysHelperMock;

        private KeyContextBuilder<User> _sut;

        public KeyContextBuilderTests()
        {
            _complexKeysHelperMock = new Mock<IComplexKeysHelper>();

            _sut = new KeyContextBuilder<User>(_complexKeysHelperMock.Object);
        }

        [Fact]
        public void AddKey_WhenCalled_IncludesKeyInContext()
        {
            const string key = "key";

            _sut.AddKey(key);

            var result = _sut.BuildRetrieveContextFromStringKey("some string");
            result.Retrieve.Should().ContainKey(key);
        }

        [Fact]
        public void BuildRetrieveContextFromStringKey_KeysCountAreGreaterThanOne_ThrowsKeyPartMissingException()
        {
            _sut.AddKey("first");
            _sut.AddKey("second");

            _sut.Invoking(_ => _.BuildRetrieveContextFromStringKey("target string"))
                .Should()
                .Throw<KeyPartMissingException>();
        }

        [Fact]
        public void BuildRetrieveContextFromStringKey_NoKeysAdded_ReturnsEmptyContext()
        {
            var result = _sut.BuildRetrieveContextFromStringKey("target string");

            result.Retrieve.Should().BeEmpty();
        }

        [Fact]
        public void BuildRetrieveContextFromStringKey_SingleKeyAdded_ReturnsSingleKeyContext()
        {
            const string key = "key";
            const string targetString = "target string";
            _sut.AddKey(key);

            var result = _sut.BuildRetrieveContextFromStringKey(targetString);

            result.Retrieve.Should().HaveCount(1)
                .And.ContainKey(key).WhoseValue.Should().Be(targetString);
        }

        [Fact]
        public void BuildRetrieveContextFromObjectKey_CallsComplexKeysHelper_WhenCalled()
        {
            _sut.BuildRetrieveContextFromObjectKey(new object());

            _complexKeysHelperMock
                .Verify(_ => _.GetProperties(typeof(object)), Times.Once);
        }

        [Fact]
        public void BuildRetrieveContextFromObjectKey_PropertyConfiguredAsKeyIsMissing_ThrowsKeyPartMissingException()
        {
            var objectKey = new { FirstKey = "Value" };
            _sut.AddKey(nameof(objectKey.FirstKey));
            _sut.AddKey("SecondKey");
            MockProperties(objectKey, nameof(objectKey.FirstKey));

            _sut.Invoking(_ => _.BuildRetrieveContextFromObjectKey(objectKey))
                .Should()
                .Throw<KeyPartMissingException>();
        }

        [Fact]
        public void BuildRetrieveContextFromObjectKey_PropertyIsNotConfiguredAsKey_SkipsSuchProperty()
        {
            var objectKey = new { FirstKey = "Value", SecondKey = "Second Value" };
            _sut.AddKey(nameof(objectKey.FirstKey));
            MockProperties(objectKey, nameof(objectKey.FirstKey), nameof(objectKey.SecondKey));

            var result = _sut.BuildRetrieveContextFromObjectKey(objectKey);

            result.Retrieve.Should().HaveCount(1)
                .And.ContainKey(nameof(objectKey.FirstKey));
        }

        [Fact]
        public void BuildRetrieveContextFromObjectKey_DefaultHappyPath_ReturnsContext()
        {
            var objectKey = new { FirstKey = "Value", SecondKey = "Second Value" };
            _sut.AddKey(nameof(objectKey.FirstKey));
            _sut.AddKey(nameof(objectKey.SecondKey));
            MockProperties(objectKey, nameof(objectKey.FirstKey), nameof(objectKey.SecondKey));

            var result = _sut.BuildRetrieveContextFromObjectKey(objectKey);

            result.Retrieve.Should().HaveCount(2)
                .And.ContainKeys(nameof(objectKey.FirstKey), nameof(objectKey.SecondKey));
        }

        [Fact]
        public void BuildCacheContext_WhenCalled_ReturnsKeyContext()
        {
            var user = new User();

            var result = _sut.BuildCacheContext(user);

            result.Store.Should().Be(user);
        }

        private void MockProperties(object key, params string[] propertyNames)
        {
            var properties = propertyNames
                .Select(n => new PropertyAccessor(n, _ => _))
                .ToArray();
            
            _complexKeysHelperMock
                .Setup(_ => _.GetProperties(key.GetType()))
                .Returns(properties);
        }
    }
}