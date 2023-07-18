using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration.PolicyBuilders;
using FluentCaching.Keys.Builders;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Configuration.PolicyBuilders;

public class PolicyNamePolicyBuilderTests
{
    [Fact]
    public void PolicyName_HappyPath_ReturnsNotNullAndPolicyBuilder()
    {
        var builder = new PolicyNamePolicyBuilder("PolicyName", new CacheOptions(new Mock<IKeyBuilder>().Object));

        var result = builder.PolicyName();

        result.Should().NotBeNull();
    }
}
