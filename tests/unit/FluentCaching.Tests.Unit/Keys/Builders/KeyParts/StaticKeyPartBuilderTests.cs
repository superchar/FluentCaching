using FluentAssertions;
using FluentCaching.Extensions;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Models;
using FluentCaching.Tests.Unit.TestModels;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders.KeyParts;

public class StaticKeyPartBuilderTests
{
    [Fact]
    public void Build_WhenCalled_BuildsKeyPart()
    {
        const string keyPart = "key part";
        var builder = Create(keyPart);

        var result = builder.Build(KeyContext.Null);
        result.Should().Be(keyPart);
    }

    [Fact]
    public void Build_ValueIsNull_ThrowsKeyPartNullException()
    {
        var builder = Create((string)null);

        var expectedMessage =
            $"Key part is null for {typeof(User).ToFullNameString()}. Please check the cache configuration.";
        builder.Invoking(b => b.Build(KeyContext.Null))
            .Should()
            .Throw<KeyPartNullException>()
            .WithMessage(expectedMessage);
    }

    [Fact]
    public void IsDynamic_WhenCalled_ReturnsFalse()
    {
        const string keyPart = "key part";
        var builder = Create(keyPart);

        builder.IsDynamic.Should().BeFalse();
    }

    private static StaticKeyPartBuilder<User> Create<T>(T value) where T : class
        => StaticKeyPartBuilder<User>.Create(value);
}