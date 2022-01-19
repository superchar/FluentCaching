using System;
using FluentAssertions;
using Xunit;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;

namespace FluentCaching.Tests.Integration.Configuration
{
    public class ConfigurationTests : BaseTest
    {
        [Fact]
        public void ForOfT_NotAPropertyExpression_ThrowsException()
        {
            Action forOfUser = () =>
                CacheBuilder
                    .For<User>(_ => _.UseAsKey(u => 1 + 1).Complete());

            forOfUser.Should().Throw<ArgumentException>()
                .WithMessage("Expression should be a single property expression");
        }
    }
}
