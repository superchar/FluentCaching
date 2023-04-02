using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Helpers;

internal interface IExpressionsHelper
{
    IReadOnlyCollection<string> GetParameterPropertyNames<TEntity, TValue>(Expression<Func<TEntity, TValue>> expression);

    Expression<Func<TEntity, string>> ReplaceResultTypeWithString<TEntity, TValue>(Expression<Func<TEntity, TValue>> expression);

    Expression<Func<Dictionary<string, object>, string>> ReplaceParameterWithDictionary<TEntity>(
        Expression<Func<TEntity, string>> expression);

    PropertyAccessor[] GetProperties(Type type);
}