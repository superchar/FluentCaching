// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace FluentCaching.Samples.DefaultCache.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class CartItemDto
{
    public Guid CartId { get; set; }
    
    public string? ProductName { get; set; }

    public int Quantity { get; set; }
}