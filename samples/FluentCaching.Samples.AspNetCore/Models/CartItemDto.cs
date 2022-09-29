namespace FluentCaching.Samples.AspNetCore.Models;

public class CartItemDto
{
    public Guid CardId { get; set; }
    
    public string ProductName { get; set; }

    public int Quantity { get; set; }
}