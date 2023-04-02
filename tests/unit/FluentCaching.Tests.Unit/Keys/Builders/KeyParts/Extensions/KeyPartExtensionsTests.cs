using System;
using FluentAssertions;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Tests.Unit.TestModels;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders.KeyParts.Extensions;

public class KeyPartExtensionsTests
{
    [Fact]
    public void ThrowIfKeyPartIsNullOrEmpty_KeyIsNull_ThrowsKeyPartMissingException()
    {
        Action throwIfKeyPartIsNullOrEmpty = () => ((string)null).ThrowIfKeyPartIsNull(typeof(User));

        throwIfKeyPartIsNullOrEmpty
            .Should()
            .Throw<KeyPartNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("key")]
    public void ThrowIfKeyPartIsNullOrEmpty_KeyIsNotNullOrEmpty_DoesNotThrowKeyPartMissingException(string key)
    {
        key.Invoking(k => k.ThrowIfKeyPartIsNull(typeof(User)))
            .Should()
            .NotThrow();
    }
}