namespace FluentCaching.Tests.Integration.Models
{
    public struct UserStructKey
    {
        public UserStructKey(int orderId, int id) => (OrderId, Id) = (orderId, id);

        public int OrderId { get; }

        public int Id { get; }
    }
}