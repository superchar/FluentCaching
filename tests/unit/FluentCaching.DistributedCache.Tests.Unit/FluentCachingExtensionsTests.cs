using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Moq;
using Xunit;

namespace FluentCaching.DistributedCache.Tests.Unit
{
    public class FluentCachingExtensionsTests
    {
        [Fact]
        public void UseFluentCaching_WhenCalled_RetunsIApplicationBuilder()
        {
            var applicationBuilderMock = new Mock<IApplicationBuilder>();

            var result = applicationBuilderMock.Object.UseFluentCaching();

            result.Should().NotBeNull();
        }
    }
}