using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Xunit;
using FluentCaching.Exceptions;
using FluentCaching.Keys;
using FluentCaching.Keys.Helpers;
using FluentCaching.Tests.Unit.Models;


namespace FluentCaching.Tests.Unit.Keys
{
    public class PropertyTrackerTests
    {
        private readonly Mock<IComplexKeysHelper> _complexKeysHelperMock;
        private readonly Mock<IExpressionsHelper> _expressionsHelperMock;

        private readonly PropertyTracker<User> _sut;

        public PropertyTrackerTests()
        {
            _complexKeysHelperMock = new Mock<IComplexKeysHelper>();
            _expressionsHelperMock = new Mock<IExpressionsHelper>();

            _sut = new PropertyTracker<User>(
                _expressionsHelperMock.Object,
                _complexKeysHelperMock.Object);
        }

        [Fact]
        public void GetStoreKey_HappyPath_DoesNotThrowException()
        {
            var user = new User { Name = "Test user" };
            _sut.Invoking(s => s.GetStoreKey(user)).Should().NotThrow();
        }

        [Fact]
        public void GetRetrieveKeySimple_SingleDynamicKeyPart_DoesNotThrowException()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));
            _sut.TrackExpression(u => u.Name);

            _sut.Invoking(s => s.GetRetrieveKeySimple("user")).Should().NotThrow();
        }

        [Fact]
        public void GetRetrieveKeySimple_NoDynamicKeyParts_DoesNotThrowException()
        {
            _sut.TrackStatic("user");

            _sut.Invoking(s => s.GetRetrieveKeySimple("user")).Should().NotThrow();
        }

        [Fact]
        public void GetRetrieveKeySimple_MultipleDynamicKeyParts_ThrowsException()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, int>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Id)));
            _sut.TrackExpression(u => u.Id);
            _sut.TrackExpression(u => u.Name);

            _sut.Invoking(s => s.GetRetrieveKeySimple("user"))
                .Should().Throw<KeyNotFoundException>().WithMessage("A single dynamic key must be defined in configuration");
        }

        [Fact]
        public void GetRetrieveKeyComplex_PropertyIsMissing_ThrowsException()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, int>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Id)));
            var propertyAccessors = new[]
            {
                new PropertyAccessor(nameof(User.Name), o => o),
            };
            _complexKeysHelperMock
                .Setup(k => k.GetProperties(It.IsAny<Type>()))
                .Returns(propertyAccessors);
            _sut.TrackExpression(u => u.Id);
            _sut.TrackExpression(u => u.Name);

            _sut.Invoking(s => s.GetRetrieveKeyComplex(new { Name = "Test user" }))
                .Should().Throw<KeyNotFoundException>().WithMessage("Key schema is not correct");
        }

        [Fact]
        public void GetRetrieveKeyComplex_PropertyIsRedundant_DoesNotThrowException()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, int>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Id)));
            var propertyAccessors = new[]
            {
                new PropertyAccessor(nameof(User.Name), o => o),
                new PropertyAccessor(nameof(User.Id), o => o),
                new PropertyAccessor("Age", o => o),
            };
            _complexKeysHelperMock
                .Setup(k => k.GetProperties(It.IsAny<Type>()))
                .Returns(propertyAccessors);
            _sut.TrackExpression(u => u.Id);
            _sut.TrackExpression(u => u.Name);

            _sut.Invoking(s => s.GetRetrieveKeyComplex(new { Name = "Test user", Id = 42, Age = 20 }))
                .Should().NotThrow();
        }

        [Fact]
        public void GetRetrieveKeyComplex_PropertiesTotallyMatchWithConfiguration_DoesNotThrowException()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, int>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Id)));
            var propertyAccessors = new[]
            {
                new PropertyAccessor(nameof(User.Name), o => o),
                new PropertyAccessor(nameof(User.Id), o => o),
            };
            _complexKeysHelperMock
                .Setup(k => k.GetProperties(It.IsAny<Type>()))
                .Returns(propertyAccessors);
            _sut.TrackExpression(u => u.Id);
            _sut.TrackExpression(u => u.Name);

            _sut.Invoking(s => s.GetRetrieveKeyComplex(new { Name = "Test user", Id = 42 }))
                .Should().NotThrow();
        }

        [Fact]
        public void TrackStatic_ValueIsNull_ThrowsException()
        {
            _sut.Invoking(s => s.TrackStatic((string)null))
                .Should().Throw<KeyPartNullException>().WithMessage("A part of a caching key cannot be null");
        }

        [Fact]
        public void TrackStatic_StoreKey_AddsValueToKey()
        {
            _sut.TrackStatic("user");

            _sut.GetStoreKey(new User()).Should().Contain("user");
        }

        [Fact]
        public void TrackStatic_RetrieveKeySimple_AddsValueToKey()
        {
            _sut.TrackStatic("user");

            _sut.GetRetrieveKeySimple("test").Should().Contain("user");
        }

        [Fact]
        public void TrackStatic_RetrieveKeyComplex_AddsValueToKey()
        {
            _sut.TrackStatic("user");

            _sut.GetRetrieveKeyComplex(new {}).Should().Contain("user");
        }

        [Fact]
        public void TrackExpression_ExpressionResultIsNull_ThrowsException()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));

            _sut.TrackExpression(u => u.Name);

            _sut.Invoking(s => s.GetStoreKey(new User()))
                .Should().Throw<KeyPartNullException>().WithMessage("A part of a caching key cannot be null");
        }

        [Fact]
        public void TrackExpression_StoreKey_AddsValueToKey()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));

            _sut.TrackExpression(u => u.Name);

            var user = new User { Name = "Test user" };
            _sut.GetStoreKey(user).Should().Contain("Test user");
        }

        [Fact]
        public void TrackExpression_RetrieveKeySimple_AddsValueToKey()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));
            _sut.TrackStatic("user_");

            _sut.TrackExpression(u => u.Name);

            _sut.GetRetrieveKeySimple("Test user").Should().Be("user_Test user");
        }

        [Fact]
        public void TrackExpression_RetrieveKeyComplex_AddsValueToKey()
        {
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, string>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Name)));
            _expressionsHelperMock
                .Setup(e => e.GetProperty(It.IsAny<Expression<Func<User, int>>>()))
                .Returns(typeof(User).GetProperty(nameof(User.Id)));
            var propertyAccessors = new[]
            {
                new PropertyAccessor(nameof(User.Name), o => o),
                new PropertyAccessor(nameof(User.Id), o => o),
            };
            _complexKeysHelperMock
                .Setup(k => k.GetProperties(It.IsAny<Type>()))
                .Returns(propertyAccessors);

            _sut.TrackExpression(u => u.Id);
            _sut.TrackExpression(u => u.Name);

            _sut.GetRetrieveKeyComplex(new { Id = 42, Name = "Test user" })
                .Should().Contain("Test user");
        }
    }
}
