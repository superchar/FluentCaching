namespace FluentCaching.Cache.Models;

public struct PolicyName
{
    public PolicyName(string name) => Name = name;
    
    public string Name { get; }
}
