using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentCaching.Keys.Helpers
{
    internal class ExpressionsHelper : IExpressionsHelper
    {
        public MemberInfo GetProperty<T, TValue>(Expression<Func<T, TValue>> expression)
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
