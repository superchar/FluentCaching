namespace FluentCaching.Tests.Unit.Models
{
    public class User
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public int? SubscriptionId { get; set; }

        public Currency Currency { get; set; }
    }
}
