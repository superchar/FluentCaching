using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.DependencyInjectionExtensions;
using FluentCaching.Memory;
using FluentCaching.Samples.CachingPolicies.Models;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

const string userLatestOrder = "UserLatestOrder";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentCaching(cacheBuilder => cacheBuilder
        .For<Order>(_ => _.UseAsKey(o => o.OrderId).And().SetInfiniteExpirationTimeout()
            .And().StoreInMemory())
        .For<Order>(_ => _.UseAsKey(o => o.UserId).And(userLatestOrder).PolicyName()
            .And().SetInfiniteExpirationTimeout().And().StoreInMemory()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/orders", async ([FromBody] OrderDto orderDto, ICache cache) =>
{
    var order = MapToOrder(orderDto);

    await cache.CacheAsync(order);
    
    return order.OrderId;
});

app.MapGet("/orders/{id:guid}", async ([FromRoute] Guid id, ICache cache) =>
{
    var order = await cache.RetrieveAsync<Order>(id);

    return order == null ? NotFound() : Ok(MapToOrderDto(order));
});

app.MapPost("/orders/{id:guid}/checkouts", async ([FromRoute] Guid id, ICache cache) =>
{
    var order = await cache.RetrieveAsync<Order>(id);
    if (order == null)
    {
        return NotFound();
    }
    
    await cache.RemoveAsync<Order>(id);
    await cache.CacheAsync(order, new PolicyName(userLatestOrder));
    
    return Ok();
});

app.MapGet("/users/{userId:guid}/latest-order", async ([FromRoute] Guid userId, ICache cache) =>
{
    var order = await cache.RetrieveAsync<Order>(userId, new PolicyName(userLatestOrder));
    
    return order == null ? NotFound() : Ok(order);
});

static Order MapToOrder(OrderDto orderDto)
    => new(Guid.NewGuid(),
        orderDto.UserId,
        orderDto.CreatedAt,
        orderDto.OrderLines
            .Select(l => new OrderLine(l.ProductId, l.Quantity))
            .ToList());

static OrderDto MapToOrderDto(Order order)
    => new(Guid.NewGuid(),
        order.UserId,
        order.CreatedAt,
        order.OrderLines
            .Select(l => new OrderLineDto(l.ProductId, l.Quantity))
            .ToList());

app.Run();
