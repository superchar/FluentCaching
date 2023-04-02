using FluentAssertions;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Builders.Factories;
using FluentCaching.Tests.Unit.TestModels;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders.Factories;

public class KeyBuilderFactoryTests
{
    [Fact]
    public void CreateKeyBuilder_WhenCalled_ReturnsKeyBuilder()
    {
        var factory = new KeyBuilderFactory();

        var result = factory.CreateKeyBuilder<User>();

        result.Should().BeOfType<KeyBuilder>();
    }
}