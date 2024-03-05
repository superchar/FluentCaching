using FluentCaching.Cache;
using FluentCaching.DependencyInjectionExtensions;
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
    .For<Cart>(_ => _.UseAsKey(c => $"cart-{c.Id}").And().SetExpirationTimeoutTo(5).Minutes
        .With().SlidingExpiration().And().StoreInMemory()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/cart-items", async ([FromBody] CartItemDto dto, ICache cache) =>
{
    var cart = await cache.RetrieveAsync<Cart>(dto.CartId) ?? new Cart(dto.CartId);
    var existingItem = cart.Items.FirstOrDefault(i =>
        i.ProductName?.Equals(dto.ProductName, StringComparison.InvariantCultureIgnoreCase) == true);
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

app.MapGet("/{cartId:guid}cart-items", (Guid cartId, ICache cache) => cache.RetrieveAsync<Cart>(cartId));

app.Run();