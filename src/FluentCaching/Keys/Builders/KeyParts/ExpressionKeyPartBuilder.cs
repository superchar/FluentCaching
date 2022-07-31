using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

internal class ExpressionKeyPartBuilder<T> : IKeyPartBuilder<T>
    where T : class
{
    private Func<T, string> _storeFunc;
    private Func<Dictionary<string, object>, string> _retrieveFunc;
    
    private readonly IExpressionsHelper _expressionsHelper;

    private ExpressionKeyPartBuilder(IExpressionsHelper expressionsHelper)
    {
        _expressionsHelper = expressionsHelper;
    }

    public bool IsDynamic => true;

    public static ExpressionKeyPartBuilder<T> Create<TValue>(Expression<Func<T, TValue>> valueGetter,
        IExpressionsHelper expressionsHelper)
        => new ExpressionKeyPartBuilder<T>(expressionsHelper)
            .AddExpression(valueGetter);

    private ExpressionKeyPartBuilder<T> AddExpression<TValue>(Expression<Func<T, TValue>> valueGetter)
    {
        var storeExpression = _expressionsHelper.ReplaceResultTypeWithString(valueGetter);
        var retrieveExpression = _expressionsHelper.ReplaceParameterWithDictionary(storeExpression);

        _storeFunc = storeExpression.Compile();
        _retrieveFunc = retrieveExpression.Compile();

        return this;
    }

    public string Build(KeyContext<T> keyContext)
    {
        var keyPart = keyContext.Store != null 
            ? _storeFunc(keyContext.Store) : _retrieveFunc(keyContext.Retrieve);

        return keyPart
            .ThrowIfKeyPartIsNullOrEmpty();
    }
}