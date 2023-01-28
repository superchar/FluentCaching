namespace FluentCaching.Tests.Integration.Models;


public struct UserStructKey
{
    public UserStructKey(int orderId, int id) => (OrderId, Id) = (orderId, id);
    
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int OrderId { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int Id { get; }
}