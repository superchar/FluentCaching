namespace FluentCaching.Samples.AspNetCore.Models;

public class Cart
{
    public Guid Id { get; set; }
    
    public List<CartItem> Items { get; set; }
}