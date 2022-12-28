using System;
using FluentAssertions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Models;
using FluentCaching.Tests.Unit.Models;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders.KeyParts;

public class StaticKeyPartBuilderTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Create_ValueIsNullOrEmpty_ThrowsKeyPartMissingException(string missingValue)
    {
        Action create = () => Create(missingValue);

        create
            .Should()
            .Throw<KeyPartMissingException>();
    }
    
    [Fact]
    public void Build_WhenCalled_BuildsKeyPart()
    {
        const string keyPart = "key part";
        var builder = Create(keyPart);

        var result = builder.Build(KeyContext.Null);
        result.Should().Be(keyPart);
    }


    private static StaticKeyPartBuilder Create<T>(T value) where T : class
        => StaticKeyPartBuilder.Create(value);
}