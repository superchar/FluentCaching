using System;
using System.Linq.Expressions;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders.KeyParts.Factories;

internal class KeyPartBuilderFactory : IKeyPartBuilderFactory
{
    private readonly IExpressionsHelper _expressionsHelper;

    public KeyPartBuilderFactory(IExpressionsHelper expressionsHelper)
    {
        _expressionsHelper = expressionsHelper;
    }

    public IKeyPartBuilder Create<TEntity, TValue>(TValue value)
        => StaticKeyPartBuilder<TEntity>.Create(value);

    public IKeyPartBuilder Create<TEntity, TValue>(Expression<Func<TEntity, TValue>> valueGetter)
        => ExpressionKeyPartBuilder<TEntity>.Create(valueGetter, _expressionsHelper);
}