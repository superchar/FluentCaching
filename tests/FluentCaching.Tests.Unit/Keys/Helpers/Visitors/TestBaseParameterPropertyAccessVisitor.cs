using System;
using System.Linq.Expressions;
using FluentCaching.Keys.Helpers.Visitors;

namespace FluentCaching.Tests.Unit.Keys.Helpers.Visitors;

public class TestBaseParameterPropertyAccessVisitor : BaseParameterPropertyAccessVisitor
{
    private readonly Action<MemberExpression> _visitCallback;

    public TestBaseParameterPropertyAccessVisitor(Action<MemberExpression> visitCallback)
    {
        _visitCallback = visitCallback;
    }

    protected override Expression VisitParameterPropertyAccess(MemberExpression node)
    {
        _visitCallback(node);
        
        return node;
    }

    public static (string, Type) GetPropertyMetadata(MemberExpression node)
        => BaseParameterPropertyAccessVisitor.GetPropertyMetadata(node);
}