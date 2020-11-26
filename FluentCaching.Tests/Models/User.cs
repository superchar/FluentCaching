using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCaching.Tests.Models
{
    public class User
    {
        public static User Test { get; } = new User {FirstName = "John", Id = 1, LastName = "Doe"};

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
