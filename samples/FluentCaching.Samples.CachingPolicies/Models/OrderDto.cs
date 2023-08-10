namespace FluentCaching.Samples.CachingPolicies.Models;

public record OrderLineDto(Guid ProductId, decimal Quantity);

public record OrderDto(Guid OrderId, Guid UserId, DateTime CreatedAt, List<OrderLineDto> OrderLines);
