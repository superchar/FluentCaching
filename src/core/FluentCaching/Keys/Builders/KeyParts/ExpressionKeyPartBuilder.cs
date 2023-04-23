using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

internal class ExpressionKeyPartBuilder<TEntity>: IKeyPartBuilder
{
    private static readonly Type EntityType = typeof(TEntity);
    
    private Func<object, string> _storeFunc = null!;
    private Func<Dictionary<string, object>, string> _retrieveFunc = null!;
    
    private readonly IExpressionsHelper _expressionsHelper;

    private ExpressionKeyPartBuilder(IExpressionsHelper expressionsHelper)
    {
        _expressionsHelper = expressionsHelper;
    }

    public bool IsDynamic => true;

    public static ExpressionKeyPartBuilder<TEntity> Create<TValue>(Expression<Func<TEntity, TValue>> valueGetter,
        IExpressionsHelper expressionsHelper)
        => new ExpressionKeyPartBuilder<TEntity>(expressionsHelper)
            .AddExpression(valueGetter);

    private ExpressionKeyPartBuilder<TEntity> AddExpression<TValue>(
        Expression<Func<TEntity, TValue>> valueGetter)
    {
        var storeExpression = _expressionsHelper.ReplaceResultTypeWithString(valueGetter);
        var retrieveExpression = _expressionsHelper.ReplaceParameterWithDictionary(storeExpression);

        var compliedStoreExpression = storeExpression.Compile();
        _storeFunc = o => compliedStoreExpression((TEntity)o); 
        _retrieveFunc = retrieveExpression.Compile();

        return this;
    }

    public string Build(KeyContext keyContext)
    {
        var keyPart = keyContext.Store != null 
            ? _storeFunc(keyContext.Store) : _retrieveFunc(keyContext.Retrieve!);

        return keyPart.ThrowIfKeyPartIsNull(EntityType);
    }
}