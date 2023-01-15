namespace FluentCaching.Tests.Integration.Models;

public class Order
{
    public static readonly Order Test = new () { OrderId = 1 };

    public int OrderId { get; set; }
}