using System;
using FluentAssertions;
using FluentCaching.Tests.Extensions;
using FluentCaching.Tests.Models;
using Xunit;

namespace FluentCaching.Tests.Cache
{
    public class ConfigurationTests : BaseTest
    {
        [Fact]
        public void ForOfT_NotAPropertyExpression_ThrowsException()
        {
            Action forOfUser = () =>
                Configuration
                    .For<User>(_ => _.UseAsKey(u => 1 + 1).Complete());

            forOfUser.Should().Throw<ArgumentException>()
                .WithMessage("Expression should be a single property expression");
        }
    }
}
