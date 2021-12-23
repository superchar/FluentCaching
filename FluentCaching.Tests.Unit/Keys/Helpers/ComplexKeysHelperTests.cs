using FluentAssertions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;
using System.Linq;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Helpers
{
    public class ComplexKeysHelperTests
    {
        private readonly ComplexKeysHelper _sut = new ComplexKeysHelper();

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

            result.Should().HaveCount(1);
            var property = result.Single();
            property.Should().NotBeNull();
            property.Name.Should().Be("Name");
        }
    }
}
