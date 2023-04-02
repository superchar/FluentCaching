namespace FluentCaching.Tests.Unit.TestModels;

public class User
{
    public string Name { get; init; }

    public int Id { get; init; }

    public int? SubscriptionId { get; init; }

    public Currency Currency { get; init; }
}