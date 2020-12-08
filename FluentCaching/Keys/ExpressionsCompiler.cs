
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FluentCaching.Keys
{
    internal static class ExpressionsCompiler
    {
        private static readonly ConcurrentDictionary<string, Delegate> CachedExpressions =
            new ConcurrentDictionary<string, Delegate>();

        public static Func<T, TValue> Compile<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression property when property.Member.MemberType == MemberTypes.Property:
                    var key = $"{property.Member.Name}_{property.Member.DeclaringType.FullName}";
                    return CachedExpressions.GetOrAdd(key, k => expression.Compile()) as Func<T, TValue>;
                default:
                    throw new ArgumentException("Expression should be a single property expression");
            }
        }
    }
}
