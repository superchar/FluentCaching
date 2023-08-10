namespace FluentCaching.Samples.DistributedCache.Models;

public class UserCheckoutStatistics
{
    public UserCheckoutStatistics(Guid userId)
    {
        UserId = userId;
    }
        
    public Guid UserId { get; }
        
    public int CheckoutCount { get; set; }
}