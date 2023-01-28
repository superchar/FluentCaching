using System.Linq;
using FluentAssertions;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders;

public class KeyContextBuilderTests
{
    private readonly Mock<IExpressionsHelper> _expressionHelperMock;

    private readonly KeyContextBuilder _sut;

    public KeyContextBuilderTests()
    {
        _expressionHelperMock = new Mock<IExpressionsHelper>();

        _sut = new KeyContextBuilder(_expressionHelperMock.Object);
    }

    [Fact]
    public void AddKey_WhenCalled_IncludesKeyInContext()
    {
        const string key = "key";

        _sut.AddKey(key);

        var result = _sut.BuildRetrieveContextFromScalarKey("some string");
        result.Retrieve.Should().ContainKey(key);
    }

    [Fact]
    public void BuildRetrieveContextFromScalarKey_KeysCountAreGreaterThanOne_ThrowsKeyPartMissingException()
    {
        _sut.AddKey("first");
        _sut.AddKey("second");

        _sut.Invoking(_ => _.BuildRetrieveContextFromScalarKey("target string"))
            .Should()
            .Throw<KeyPartMissingException>();
    }

    [Fact]
    public void BuildRetrieveContextFromScalarKey_NoKeysAdded_ReturnsEmptyContext()
    {
        var result = _sut.BuildRetrieveContextFromScalarKey("target string");

        result.Retrieve.Should().BeEmpty();
    }

    [Fact]
    public void BuildRetrieveContextFromScalarKey_SingleKeyAdded_ReturnsSingleKeyContext()
    {
        const string key = "key";
        const string targetString = "target string";
        _sut.AddKey(key);

        var result = _sut.BuildRetrieveContextFromScalarKey(targetString);

        result.Retrieve.Should().HaveCount(1)
            .And.ContainKey(key).WhoseValue.Should().Be(targetString);
    }

    [Fact]
    public void BuildRetrieveContextFromComplexKey_CallsComplexKeysHelper_WhenCalled()
    {
        _sut.BuildRetrieveContextFromComplexKey(new object());

        _expressionHelperMock
            .Verify(_ => _.GetProperties(typeof(object)), Times.Once);
    }

    [Fact]
    public void BuildRetrieveContextFromComplexKey_PropertyConfiguredAsKeyIsMissing_ThrowsKeyPartMissingException()
    {
        var objectKey = new { FirstKey = "Value" };
        _sut.AddKey(nameof(objectKey.FirstKey));
        _sut.AddKey("SecondKey");
        MockProperties(objectKey, nameof(objectKey.FirstKey));

        _sut.Invoking(_ => _.BuildRetrieveContextFromComplexKey(objectKey))
            .Should()
            .Throw<KeyPartMissingException>();
    }

    [Fact]
    public void BuildRetrieveContextFromComplexKey_PropertyIsNotConfiguredAsKey_SkipsSuchProperty()
    {
        var objectKey = new { FirstKey = "Value", SecondKey = "Second Value" };
        _sut.AddKey(nameof(objectKey.FirstKey));
        MockProperties(objectKey, nameof(objectKey.FirstKey), nameof(objectKey.SecondKey));

        var result = _sut.BuildRetrieveContextFromComplexKey(objectKey);

        result.Retrieve.Should().HaveCount(1)
            .And.ContainKey(nameof(objectKey.FirstKey));
    }

    [Fact]
    public void BuildRetrieveContextFromComplexKey_DefaultHappyPath_ReturnsContext()
    {
        var objectKey = new { FirstKey = "Value", SecondKey = "Second Value" };
        _sut.AddKey(nameof(objectKey.FirstKey));
        _sut.AddKey(nameof(objectKey.SecondKey));
        MockProperties(objectKey, nameof(objectKey.FirstKey), nameof(objectKey.SecondKey));

        var result = _sut.BuildRetrieveContextFromComplexKey(objectKey);

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
            
        _expressionHelperMock
            .Setup(_ => _.GetProperties(key.GetType()))
            .Returns(properties);
    }
}