namespace FluentCaching.Samples.DefaultCache.Models;

public class Cart
{
    public Cart(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; }

    public List<CartItem> Items { get; } = new ();
}