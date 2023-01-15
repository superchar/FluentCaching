using System.Collections.Generic;
using FluentAssertions;
using FluentCaching.Keys.Extensions;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Extensions;

public class DictionaryExtensionsTests
{
    [Fact]
    public void FirstKey_WhenCalled_ReturnsFirstKey()
    {
        var dictionary = new Dictionary<string, string>
        {
            { "first key", "first value" },
            { "second key", "second value" }
        };

        var key = dictionary.FirstKey();

        key.Should().Be("first key");
    }
}