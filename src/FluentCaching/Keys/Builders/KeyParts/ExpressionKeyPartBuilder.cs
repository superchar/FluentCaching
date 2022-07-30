using System;
using System.Linq.Expressions;
using FluentCaching.Keys.Builders.KeyParts.Extensions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

internal class ExpressionKeyPartBuilder<T> : IKeyPartBuilder<T>
    where T : class
{
    private string _property;

    private Func<T, string> _compiledExpression;

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
        _property = _expressionsHelper.GetPropertyName(valueGetter);
        _compiledExpression = _expressionsHelper
            .RewriteWithSafeToString(valueGetter)
            .Compile();

        return this;
    }

    public string Build(KeyContext<T> keyContext)
    {
        var keyPart = keyContext.Store != null
            ? _compiledExpression(keyContext.Store)
            : keyContext.Retrieve[_property]?.ToString();

        return keyPart
            .ThrowIfKeyPartIsNullOrEmpty();
    }
}