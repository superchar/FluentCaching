namespace FluentCaching.Tests.Integration.Models;

public class User
{
    public static User Test { get; } = new () { FirstName = "John", Id = 1, LastName = "Doe" };

    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public override string ToString() => $"{FirstName}_{LastName}";

    public Address Address { get; set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Order LastOrder { get; set; }
}