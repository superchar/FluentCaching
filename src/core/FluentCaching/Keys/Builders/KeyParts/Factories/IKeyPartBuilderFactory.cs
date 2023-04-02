using System;
using System.Linq.Expressions;

namespace FluentCaching.Keys.Builders.KeyParts.Factories;

internal interface IKeyPartBuilderFactory
{
    IKeyPartBuilder Create<TEntity, TValue>(TValue value);
    
    IKeyPartBuilder Create<TEntity, TValue>(Expression<Func<TEntity, TValue>> valueGetter);
}