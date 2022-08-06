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

    public IKeyPartBuilder Create<TValue>(TValue value)
        => StaticKeyPartBuilder.Create(value);

    public IKeyPartBuilder Create<T, TValue>(Expression<Func<T, TValue>> valueGetter)
        => ExpressionKeyPartBuilder.Create(valueGetter, _expressionsHelper);
}