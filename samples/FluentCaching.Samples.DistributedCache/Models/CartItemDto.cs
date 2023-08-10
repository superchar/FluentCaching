// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace FluentCaching.Samples.DistributedCache.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class CartItemDto
{
    public Guid CartId { get; set; }
    
    public string ProductName { get; set; }

    public int Quantity { get; set; }
}
