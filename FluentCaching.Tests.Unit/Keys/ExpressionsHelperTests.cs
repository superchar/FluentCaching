using FluentAssertions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Cache.Models;
using System;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys
{
    public class ExpressionsHelperTests
    {
        private readonly ExpressionsHelper _sut = new ExpressionsHelper();

        [Fact]
        public void GetProperty_ExpressionIsNotSingleProperty_ThrowsException()
        {
            _sut.Invoking(s => s.GetProperty<User, string>(u => "test"))
                .Should().Throw<ArgumentException>().WithMessage("Expression should be a single property expression");
        }

        [Fact]
        public void GetProperty_ExpressionIsSingleProperty_ReturnsMemberyInfo()
        {
            var result = _sut.GetProperty<User, string>(u => u.Name);

            result.Should().NotBeNull();
            result.Name.Should().Be("Name");
        }
    }
}
