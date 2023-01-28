namespace FluentCaching.Tests.Integration.Models;

public struct Address
{
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public int Id { get; }
        
    public string Street { get; init; }
}