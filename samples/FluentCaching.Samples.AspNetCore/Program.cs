using FluentCaching.Cache;
using FluentCaching.DependencyInjectionExtensions;
using FluentCaching.Memory;
using FluentCaching.Samples.AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentCaching(cacheBuilder => cacheBuilder
        .For<Cart>(_ => _.UseAsKey(c => c.Id).And().WithTtlOf(5).Minutes
                .And().SlidingExpiration().And().UseInMemoryCache()));

var app = builder.Build();

app.MapPost("/card-items", async ([FromBody] CartItemDto dto, ICache cache) =>
{
    var cart = await cache.RetrieveAsync<Cart>() ?? new Cart();
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

app.MapGet("/card-items", (ICache cache) => cache.RetrieveAsync<Cart>());

app.Run();