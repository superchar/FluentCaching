using FluentAssertions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Helpers
{
    public class ComplexKeysHelperTests
    {
        private readonly ComplexKeysHelper _sut = new ();

        [Fact]
        public void GetProperties_TypeHasNoProperties_ReturnsEmptyArray()
        {
            var result = _sut.GetProperties(typeof(TypeWithoutProperties));

            result.Should().BeEmpty();
        }

        [Fact]
        public void GetProperties_TypeHasProperties_ReturnsPropertiesArray()
        {
            var result = _sut.GetProperties(typeof(User));

            result.Should().HaveCount(4);
            result.Should().Contain(e => e.Name == nameof(User.Name))
                .And.Contain(e => e.Name == nameof(User.Id))
                .And.Contain(e => e.Name == nameof(User.Currency))
                .And.Contain(e => e.Name == nameof(User.SubscriptionId));
        }
    }
}
