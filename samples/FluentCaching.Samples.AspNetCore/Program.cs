using FluentCaching.Cache;
using FluentCaching.DependencyInjectionExtensions;
using FluentCaching.DistributedCache;
using FluentCaching.Memory;
using FluentCaching.Samples.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
});

builder.Services.AddFluentCaching(cacheBuilder => cacheBuilder
    .For<Cart>(_ => _.UseAsKey(c => $"card-{c.Id}").And().SetExpirationTimeoutTo(5).Minutes
        .With().SlidingExpiration().And().StoreInMemory())
    .For<UserCheckoutStatistics>(_ => _.UseClassNameAsKey().CombinedWith(s => s.UserId)
        .And().SetInfiniteExpirationTimeout().And().StoreInDistributedCache()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseFluentCaching();

app.MapPost("/card-items", async ([FromBody] CartItemDto dto, ICache cache) =>
{
    var cart = await cache.RetrieveAsync<Cart>(dto.CardId) ?? new Cart(dto.CardId);
    var existingItem = cart.Items.FirstOrDefault(i =>
        i.ProductName.Equals(dto.ProductName, StringComparison.InvariantCultureIgnoreCase));
    if (existingItem != null)
    {
        existingItem.Quantity += dto.Quantity;
    }
    else
    {
        cart.Items.Add(new CartItem(dto.ProductName, dto.Quantity));
    }

    await cache.CacheAsync(cart);
});

app.MapGet("/{cardId:guid}/card-items", (Guid cardId, ICache cache) => cache.RetrieveAsync<Cart>(cardId));

app.MapPost("/{cardId:guid}/checkouts", async (Guid cardId, [FromQuery] Guid userId, ICache cache) =>
{
    await cache.RemoveAsync<Cart>(cardId);
    var checkoutStatistics = await cache.RetrieveAsync<UserCheckoutStatistics>(userId)
                             ?? new UserCheckoutStatistics(userId);
    checkoutStatistics.CheckoutCount++;
    await cache.CacheAsync(checkoutStatistics);
});

app.MapGet("/{userId:guid}/checkout-statistics",
    (Guid userId, ICache cache) => cache.RetrieveAsync<UserCheckoutStatistics>(userId));


app.Run();