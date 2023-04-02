using System;
using FluentAssertions;
using FluentCaching.Extensions;
using FluentCaching.Tests.Unit.TestModels;
using Xunit;

namespace FluentCaching.Tests.Unit.Extensions;

public class TypeExtensionsTests
{
    [Theory]
    [InlineData(typeof(User), "type - 'FluentCaching.Tests.Unit.TestModels.User'")]
    [InlineData(typeof(Currency), "type - 'FluentCaching.Tests.Unit.TestModels.Currency'")]
    [InlineData(typeof(TypeWithoutProperties), "type - 'FluentCaching.Tests.Unit.TestModels.TypeWithoutProperties'")]
    public void ToFullNameString_WhenProvidedType_ReturnsFullNameString(Type type, string expectedString)
    {
        var result = type.ToFullNameString();

        result.Should().Be(expectedString);
    }
}