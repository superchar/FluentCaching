using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Extensions;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;
using Xunit;

namespace FluentCaching.Tests.Integration.Keys;

public class KeyGenerationTests : BaseTest
{
    private readonly User _user;

    public KeyGenerationTests()
    {
        var fixture = new Fixture();
        fixture.Customize(new SupportMutableValueTypesCustomization());
        _user = fixture.Create<User>();
    }

    [Fact]
    public async Task StaticKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey("test").Complete());

        var cache = await CacheAsync(new User());
        CacheImplementation.Dictionary.Should().ContainKey("test");

        await cache.RemoveAsync<User>("test");
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task PropertyReferenceTypeKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.FirstName).Complete());
        _user.FirstName = "Test User";

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("Test User");

        await cache.RemoveAsync<User>("Test User");
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task PropertyValueTypeKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.Id).Complete());
        _user.Id = 42;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("42");

        await cache.RemoveAsync<User>(42);
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task NestedPropertyValueTypeKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.Address.Street).Complete());
        _user.Address = new Address { Street = "Some street" };

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("Some street");

        await cache.RemoveAsync<User>("Some street");
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task NestedPropertyReferenceTypeKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.LastOrder.OrderId).Complete());
        _user.LastOrder.OrderId = 43;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("43");

        await cache.RemoveAsync<User>(43);
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task StaticKeyCombinedWithPropertyReferenceTypeKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey("User").CombinedWith(u => u.LastName).Complete());
        _user.LastName = "Dou";

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("User:Dou");

        await cache.RemoveAsync<User>("Dou");
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task StaticKeyCombinedWithPropertyValueTypeKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey("User").CombinedWith(u => u.Id).Complete());
        _user.Id = 42;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("User:42");

        await cache.RemoveAsync<User>(42);
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task StaticKeyCombinedWithNestedValueTypeProperty_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey("User")
            .CombinedWith(u => u.Address.Street).Complete());
        _user.Address = new Address { Street = "Some street" };

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("User:Some street");

        await cache.RemoveAsync<User>("Some street");
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task StaticKeyCombinedWithNestedReferenceTypeProperty_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey("User")
            .CombinedWith(u => u.LastOrder.OrderId).Complete());
        _user.LastOrder.OrderId = 45;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("User:45");

        await cache.RemoveAsync<User>(45);
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task MultipleReferenceTypePropertiesKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => $"{u.FirstName} {u.LastName}").Complete());
        _user.FirstName = "John";
        _user.LastName = "Dou";

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("John Dou");

        await cache.RemoveAsync<User>(new { FirstName = "John", LastName = "Dou" });
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task MultipleNestedPropertiesKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.Address.Street)
            .CombinedWith(u => u.LastOrder.OrderId).Complete());
        _user.Address = new Address { Street = "Street" };
        _user.LastOrder.OrderId = 47;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("Street:47");

        await cache.RemoveAsync<User>(new { Street = "Street", OrderId = 47 });
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task ArithmeticOperationKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.Id + u.LastOrder.OrderId).Complete());
        _user.Id = 9;
        _user.Address = new Address { Street = "Street" };
        _user.LastOrder.OrderId = 4;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("13");

        await cache.RemoveAsync<User>(new { Id = 9, OrderId = 4 });
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task ClosureKey_GeneratesCorrectKey()
    {
        var orderFromClosure = new Order
        {
            OrderId = 42
        };
        CacheBuilder.For<User>(_ => _.UseAsKey(u => u.LastOrder.OrderId + orderFromClosure.OrderId).Complete());
        _user.LastOrder.OrderId = 4;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("46");

        await cache.RemoveAsync<User>(4);
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task ExtensionMethodKey_GeneratesCorrectKey()
    {
        CacheBuilder.For<User>(_ =>
            _.UseAsKey(u => u.LastOrder.OrderId.MultiplyByTwo() + u.Id.MultiplyByTwo()).Complete());
        _user.LastOrder.OrderId = 4;
        _user.Id = 9;

        var cache = await CacheAsync(_user);
        CacheImplementation.Dictionary.Should().ContainKey("26");

        await cache.RemoveAsync<User>(new { OrderId = 4, Id = 9 });
        CacheImplementation.Dictionary.Should().BeEmpty();
    }

    [Fact]
    public async Task StructRetrieveKey_CanRetrieveWithStructKey()
    {
        CacheBuilder.For<User>(_ =>
            _.UseAsKey(u => u.LastOrder.OrderId + u.Id).Complete());
        _user.LastOrder.OrderId = 4;
        _user.Id = 9;

        var cache = await CacheAsync(_user);

        var result = await cache.RetrieveAsync<User>(new UserStructKey(4, 9));
        result.Should().Be(_user);
    }

    [Fact]
    public void MultipleNestedPropertiesWithTheSameName_ShouldThrowException()
    {
        var expectedMessage = $"The caching key for {typeof(User).ToFullNameString()} cannot contain multiple properties with the same name. " +
                                       "Duplicated property name - 'Id'.";
        CacheBuilder.Invoking(c => c.For<User>(_ => _.UseAsKey(u => u.Address.Id)
                .CombinedWith(u => u.Id).Complete()))
            .Should().Throw<KeyPropertyDuplicateException>()
            .WithMessage(expectedMessage);
    }

    private async Task<ICache> CacheAsync(User user)
    {
        var cache = CacheBuilder.Build();
        await cache.CacheAsync(user);

        return cache;
    }
}