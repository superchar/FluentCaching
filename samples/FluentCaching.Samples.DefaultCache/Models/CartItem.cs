namespace FluentCaching.Samples.DefaultCache.Models;

public class CartItem
{
    public CartItem(string? productName, int quantity)
    {
        ProductName = productName;
        Quantity = quantity;
    }
    
    public string? ProductName { get; }
    
    public int Quantity { get; set; }
}