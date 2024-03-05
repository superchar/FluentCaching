namespace FluentCaching.Samples.DistributedCache.Models;

public class Cart(Guid id)
{
    public Guid Id { get; } = id;

    public List<CartItem> Items { get; } = [];
}
