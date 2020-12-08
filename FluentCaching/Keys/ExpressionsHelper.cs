
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FluentCaching.Keys
{
    internal static class ExpressionsHelper
    {
        private static readonly ConcurrentDictionary<string, Delegate> CachedExpressions =
            new ConcurrentDictionary<string, Delegate>();

        public static Func<T, TValue> Compile<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var property = GetProperty(expression);
            
            var key = $"{property.Name}_{property.DeclaringType.FullName}";

            return CachedExpressions.GetOrAdd(key, k => expression.Compile()) as Func<T, TValue>;
        }

        public static MemberInfo GetProperty<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression property when property.Member.MemberType == MemberTypes.Property:
                    return property.Member;
                default:
                    throw new ArgumentException("Expression should be a single property expression");
            }
        }

    }
}
