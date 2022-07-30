using System;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders.KeyParts.Extensions;

public class KeyPartExtensionsTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ThrowIfKeyPartIsNullOrEmpty_KeyIsNullOrEmpty_ThrowsKeyPartMissingException(string key)
    {
        Action throwIfKeyPartIsNullOrEmpty = () => key.ThrowIfKeyPartIsNullOrEmpty();
        
        throwIfKeyPartIsNullOrEmpty
            .Should()
            .Throw<KeyPartMissingException>();
    }
    
    [Fact]
    public void ThrowIfKeyPartIsNullOrEmpty_KeyIsNotNullOrEmpty_DoesNotThrowKeyPartMissingException()
    {
        const string key = "key";

        key.Invoking(_ => _.ThrowIfKeyPartIsNullOrEmpty())
            .Should()
            .NotThrow();
    }
}