using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders.KeyParts.Factories;

internal interface IKeyPartBuilderFactory<T>
    where T : class
{
    IKeyPartBuilder<T> Create<TValue>(TValue value);
    
    IKeyPartBuilder<T> Create<TValue>(Expression<Func<T, TValue>> valueGetter);
}