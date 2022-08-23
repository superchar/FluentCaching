using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Helpers.Visitors;

public class CollectParameterPropertyNamesVisitor : BaseParameterPropertyAccessVisitor
{
    private readonly List<string> _properties = new();

    public IReadOnlyCollection<string> Properties => _properties;
    
    protected override Expression VisitParameterPropertyAccess(MemberExpression node)
    {
        var (propertyName, _) = GetPropertyMetadata(node);
        _properties.Add(propertyName);

        return node;
    }
}