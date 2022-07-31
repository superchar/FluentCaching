using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentCaching.Keys.Helpers.Visitors;

namespace FluentCaching.Keys.Helpers;

public class ReplaceParameterWithDictionaryVisitor : BaseParameterPropertyAccessVisitor
{
    public ReplaceParameterWithDictionaryVisitor(Expression dictionaryParam)
    {
        _dictionaryParam = dictionaryParam;
    }

    private readonly Expression _dictionaryParam;

    protected override Expression VisitParameter(ParameterExpression node)
        => _dictionaryParam;

    protected override Expression VisitParameterPropertyAccess(MemberExpression node)
    {
        var (propertyName, propertyType) = GetPropertyMetadata(node);
        var lookupResult = GenerateDictionaryLookup(propertyName);
        var convertedLookupResult = Expression.Convert(lookupResult, propertyType);

        return convertedLookupResult;
    }

    private static (string, Type) GetPropertyMetadata(MemberExpression node)
    {
        var propertyInfo = (PropertyInfo)node.Member;

        return (propertyInfo.Name, propertyInfo.PropertyType);
    }

    private Expression GenerateDictionaryLookup(string propertyName)
    {
        var dictionaryIndexer = _dictionaryParam.Type.GetProperty("Item");
        var propertyNameKey = Expression.Constant(propertyName);
        
        return Expression.Property(_dictionaryParam, dictionaryIndexer, propertyNameKey);
    }
}