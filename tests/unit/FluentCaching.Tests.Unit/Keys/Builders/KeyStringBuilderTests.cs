using System.Linq;
using AutoFixture;
using FluentAssertions;
using FluentCaching.Keys.Builders;
using FluentCaching.Keys.Exceptions;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Builders
{
    public class KeyStringBuilderTests
    {
        private static readonly Fixture Fixture = new Fixture();
        
        private KeyStringBuilder _sut;

        public KeyStringBuilderTests()
        {
            _sut = new KeyStringBuilder();
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(33)]
        [InlineData(61)]
        [InlineData(120)]
        public void Append_StringIsLessThenMaxSize_DoesNotThrowException(int count)
        {
            _sut.Invoking(_ => AppendStrings(count)).Should().NotThrow();
        }
        
        [Fact]
        public void Append_StringIsMoreThenMaxSize_ThrowsKeyLengthExceededException()
        {
            _sut.Invoking(_ => AppendStrings(121)).Should().Throw<KeyCountExceededException>()
                .WithMessage("Maximum key parts count exceeded. Maximum count is 120.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(33)]
        [InlineData(61)]
        [InlineData(120)]
        public void ToString_WhenCalled_ReturnsTargetString(int count)
        {
            var targetStrings = AppendStrings(count);

            var result = _sut.ToString();

            var expectedResult = string.Join(string.Empty, targetStrings);
            result.Should().Be(expectedResult);
        }
        
        private string[] AppendStrings(int count)
        {
            var targetStrings = Fixture
                .CreateMany<string>(count)
                .ToArray();
 
            foreach (var targetString in targetStrings)
            {
                _sut.Append(targetString);
            }

            return targetStrings;
        }
    }
}

