using System;

namespace FluentCaching.Tests.Integration.Models
{
    public class Order
    {
        public static readonly Order Test = new Order { CreationDate = DateTime.MaxValue, OrderId = 1 };

        public int OrderId { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
