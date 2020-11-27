

using System;
using System.Reflection.Metadata;

namespace FluentCaching.Tests.Models
{
    public class Order
    {
        public static readonly Order Test = new Order {CreationDate = DateTime.MaxValue, Id = 1};

        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
