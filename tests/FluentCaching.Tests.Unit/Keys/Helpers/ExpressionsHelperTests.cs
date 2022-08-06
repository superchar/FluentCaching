using FluentAssertions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;
using Xunit;

namespace FluentCaching.Tests.Unit.Keys.Helpers
{
    public class ExpressionsHelperTests
    {
        private readonly ExpressionsHelper _sut = new ();

        [Fact]
        public void GetProperty_ExpressionIsNotSingleProperty_DoesNotThrowException()
        {
            _sut.Invoking(s => s.GetParameterPropertyNames<User, string>(u => "test"))
                .Should().NotThrow();
        }

        [Fact]
        public void GetProperty_ExpressionWithMultipleProperties_ReturnsPropertyNames()
        {
            var result = _sut.GetParameterPropertyNames<User, int>(u => u.Id + u.SubscriptionId.Value);

            var expectedProperties = new[] { nameof(User.Id), nameof(User.SubscriptionId) };
            result.Should().BeEquivalentTo(expectedProperties);
        }

        [Fact]
        public void RewriteWithSafeToString_PropertyIsPrimitiveType_AddsToString()
        {
            var user = new User
            {
                Id = 42
            };

            var resultExpression = _sut.ReplaceResultTypeWithString<User, int>(_ => _.Id);

            var compiledExpression = resultExpression.Compile();
            var result = compiledExpression(user);
            result.Should().Be("42");
        }
        
        [Fact]
        public void RewriteWithSafeToString_PropertyIsNullablePrimitiveType_AddsToString()
        {
            var user = new User
            {
                SubscriptionId = 42
            };

            var resultExpression = _sut.ReplaceResultTypeWithString<User, int?>(_ => _.SubscriptionId);

            var compiledExpression = resultExpression.Compile();
            var result = compiledExpression(user);
            result.Should().Be("42");
        }
        
        [Fact]
        public void RewriteWithSafeToString_PropertyIsReferenceType_AddsToString()
        {
            var user = new User
            {
                Currency = new Currency("USD")
            };

            var resultExpression = _sut.ReplaceResultTypeWithString<User, Currency>(_ => _.Currency);

            var compiledExpression = resultExpression.Compile();
            var result = compiledExpression(user);
            result.Should().Be("USD");
        }
        
        [Fact]
        public void RewriteWithSafeToString_CachedObjectIsNull_AddsNullCheck()
        {
            var resultExpression = _sut.ReplaceResultTypeWithString<User, int>(_ => _.Id);

            var compiledExpression = resultExpression.Compile();
            var result = compiledExpression(null);
            result.Should().BeNull();
        }
        
        [Fact]
        public void RewriteWithSafeToString_NullablePrimitiveTypePropertyIsNull_AddsNullCheck()
        {
            var resultExpression = _sut.ReplaceResultTypeWithString<User, int?>(_ => _.SubscriptionId);

            var compiledExpression = resultExpression.Compile();
            var result = compiledExpression(new User());
            result.Should().BeNull();
        }
        
        [Fact]
        public void RewriteWithSafeToString_ReferenceTypePropertyIsNull_AddsNullCheck()
        {
            var resultExpression = _sut.ReplaceResultTypeWithString<User, Currency>(_ => _.Currency);

            var compiledExpression = resultExpression.Compile();
            var result = compiledExpression(new User());
            result.Should().BeNull();
        }
    }
}
