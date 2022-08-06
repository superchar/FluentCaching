using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders.KeyParts.Factories;

internal interface IKeyPartBuilderFactory
{
    IKeyPartBuilder Create<TValue>(TValue value);
    
    IKeyPartBuilder Create<T, TValue>(Expression<Func<T, TValue>> valueGetter);
}