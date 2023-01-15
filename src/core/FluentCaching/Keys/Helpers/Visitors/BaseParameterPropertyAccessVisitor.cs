using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentCaching.Keys.Helpers.Visitors;

public abstract class BaseParameterPropertyAccessVisitor : ExpressionVisitor
{
    protected abstract Expression VisitParameterPropertyAccess(MemberExpression node);

    protected override Expression VisitMember(MemberExpression node)
        => IsParameterPropertyAccess(node) 
            ? VisitParameterPropertyAccess(node) : base.VisitMember(node);

    protected static (string, Type) GetPropertyMetadata(MemberExpression node)
    {
        var propertyInfo = (PropertyInfo)node.Member;

        return (propertyInfo.Name, propertyInfo.PropertyType);
    }

    private static bool IsParameterPropertyAccess(MemberExpression node) =>
        node.Member.MemberType == MemberTypes.Property
        && node.NodeType == ExpressionType.MemberAccess
        && ComesFromParameter(node)
        && !IsNullableValueAccess(node);

    private static bool ComesFromParameter(Expression expression)
    {
        if (expression.NodeType == ExpressionType.Parameter)
        {
            return true;
        }

        if (expression is not MemberExpression memberExpression
            || memberExpression.Member.MemberType != MemberTypes.Property)
        {
            return false;
        }

        // ReSharper disable once TailRecursiveCall
        return ComesFromParameter(memberExpression.Expression);
    }

    private static bool IsNullableValueAccess(MemberExpression node)
        => node.Member.Name == "Value" && Nullable.GetUnderlyingType(node.Member.DeclaringType) != null;
}