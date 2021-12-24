using FluentAssertions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;
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

            result.Should().HaveCount(2);
            result.Should().Contain(e => e.Name == "Name")
                .And.Contain(e => e.Name == "Id");
        }
    }
}
