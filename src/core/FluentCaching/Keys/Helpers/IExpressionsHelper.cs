using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Helpers
{
    internal interface IExpressionsHelper
    {
        IReadOnlyCollection<string> GetParameterPropertyNames<T, TValue>(Expression<Func<T, TValue>> expression);

        Expression<Func<T, string>> ReplaceResultTypeWithString<T, TValue>(Expression<Func<T, TValue>> expression);

        Expression<Func<Dictionary<string, object>, string>> ReplaceParameterWithDictionary<T>(
            Expression<Func<T, string>> expression);

        PropertyAccessor[] GetProperties(Type type);
    }
}