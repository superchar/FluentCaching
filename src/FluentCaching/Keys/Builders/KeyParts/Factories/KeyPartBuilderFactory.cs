using System;
using System.Linq.Expressions;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders.KeyParts.Factories;

internal class KeyPartBuilderFactory<T> : IKeyPartBuilderFactory<T>
    where T : class
{
    private readonly IExpressionsHelper _expressionsHelper;

    public KeyPartBuilderFactory(IExpressionsHelper expressionsHelper)
    {
        _expressionsHelper = expressionsHelper;
    }

    public IKeyPartBuilder<T> Create<TValue>(TValue value)
        => StaticKeyPartBuilder<T>.Create(value);

    public IKeyPartBuilder<T> Create<TValue>(Expression<Func<T, TValue>> valueGetter)
        => ExpressionKeyPartBuilder<T>.Create(valueGetter, _expressionsHelper);
}