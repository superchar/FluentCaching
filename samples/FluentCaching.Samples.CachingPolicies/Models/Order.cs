namespace FluentCaching.Samples.CachingPolicies.Models;

public record OrderLine(Guid ProductId, decimal Quantity);

public record Order(Guid OrderId, Guid UserId, DateTime CreatedAt, List<OrderLine> OrderLines);
