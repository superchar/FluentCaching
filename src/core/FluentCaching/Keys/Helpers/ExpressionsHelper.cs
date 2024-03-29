﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentCaching.Keys.Helpers.Visitors;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Helpers;

internal class ExpressionsHelper : IExpressionsHelper
{
    private static readonly ConcurrentDictionary<Type, PropertyAccessor[]> Cache = new ();
    private static readonly MethodInfo CallInnerDelegateMethod =
        typeof(ExpressionsHelper).GetMethod(nameof(CallInnerDelegate),
            BindingFlags.NonPublic | BindingFlags.Static)!;

    public IEnumerable<string> GetParameterPropertyNames<TEntity, TValue>(Expression<Func<TEntity, TValue>> expression)
    {
        var visitor = new CollectParameterPropertyNamesVisitor();
        visitor.Visit(expression.Body);

        return visitor.Properties;
    }

    public Expression<Func<TEntity, string>> ReplaceResultTypeWithString<TEntity, TValue>(Expression<Func<TEntity, TValue>> expression)
    {
        var newBody = expression.Body.Type != typeof(string)
            ? IsNullableExpression(expression.Body)
                ? GenerateNullCheck(expression.Body, ifNotNull: GenerateToStringCall(expression.Body))
                : GenerateToStringCall(expression.Body)
            : expression.Body;

        return Expression.Lambda<Func<TEntity, string>>(newBody, expression.Parameters);
    }

    public Expression<Func<Dictionary<string, object>, string>> ReplaceParameterWithDictionary<TEntity>(Expression<Func<TEntity, string>> expression)
    {
        var dictionaryParam = Expression.Parameter(typeof(Dictionary<string, object>));
        var body = new ReplaceParameterWithDictionaryVisitor(dictionaryParam).Visit(expression.Body);
            
        return Expression.Lambda<Func<Dictionary<string, object>, string>>(body, dictionaryParam);
    }
        
    public PropertyAccessor[] GetProperties(Type type)
        => Cache
            .GetOrAdd(type, t => t.GetProperties()
                .Where(p => p.GetMethod != null)
                .Select(CreatePropertyAccessor)
                .ToArray());
        
    private static Delegate CreateGetPropertyDelegate(MemberInfo property)
    {
        var parameter = Expression.Parameter(property.DeclaringType!);
        var body = Expression.Property(parameter, property.Name);

        return Expression.Lambda(body, parameter)
            .Compile();
    }
        
    private static PropertyAccessor CreatePropertyAccessor(PropertyInfo property)
    {
        var getMethodDelegate = CreateGetPropertyDelegate(property);
        var callInnerGenericMethodWithTypes = CallInnerDelegateMethod
            .MakeGenericMethod(property.DeclaringType!, property.PropertyType);
        var result = (Func<object, object?>)callInnerGenericMethodWithTypes
            .Invoke(null, [getMethodDelegate])!;
        return new PropertyAccessor(property.Name, result);
    }
        
    private static Func<object, object?> CallInnerDelegate<TClass, TResult>(
        Func<TClass, TResult> targetDelegate)
        => instance => targetDelegate((TClass)instance);

    private static ConditionalExpression GenerateNullCheck(Expression expression, Expression ifNotNull)
        => Expression.Condition(
            test: Expression.Equal(expression, Expression.Constant(null)),
            ifTrue: Expression.Constant(null, typeof(string)),
            ifFalse: ifNotNull);

    private static bool IsNullableExpression(Expression expression)
        => !expression.Type.IsValueType
           || Nullable.GetUnderlyingType(expression.Type) != null;

    private static MethodCallExpression GenerateToStringCall(Expression expression)
        => Expression.Call(expression, nameof(ToString), Type.EmptyTypes);
}