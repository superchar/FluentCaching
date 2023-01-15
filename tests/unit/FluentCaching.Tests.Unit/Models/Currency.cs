namespace FluentCaching.Tests.Unit.Models;

public class Currency
{
    public Currency(string name) => Name = name;

    private string Name { get; }

    public override string ToString() => Name;
}