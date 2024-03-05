using System.Linq.Expressions;

namespace FluentCaching.Keys.Helpers.Visitors;

public class ReplaceParameterWithDictionaryVisitor : BaseParameterPropertyAccessVisitor
{
    public ReplaceParameterWithDictionaryVisitor(Expression dictionaryParam)
    {
        _dictionaryParam = dictionaryParam;
    }

    private readonly Expression _dictionaryParam;

    protected override Expression VisitParameterPropertyAccess(MemberExpression node)
    {
        var (propertyName, propertyType) = GetPropertyMetadata(node);
        var lookupResult = GenerateDictionaryLookup(propertyName);
        var convertedLookupResult = Expression.Convert(lookupResult, propertyType);

        return convertedLookupResult;
    }

    private IndexExpression GenerateDictionaryLookup(string propertyName)
    {
        var dictionaryIndexer = _dictionaryParam.Type.GetProperty("Item");
        var propertyNameKey = Expression.Constant(propertyName);
        
        return Expression.Property(_dictionaryParam, dictionaryIndexer!, propertyNameKey);
    }
}