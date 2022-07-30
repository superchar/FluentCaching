using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentCaching.Keys.Helpers
{
    internal class ExpressionsHelper : IExpressionsHelper
    {
        public string GetPropertyName<T, TValue>(Expression<Func<T, TValue>> expression)
            => GetPropertyExpression(expression).Member.Name;

        public Expression<Func<T, string>> RewriteWithSafeToString<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var nullableProperty = ConvertToNullableExpression(
                GetPropertyExpression<T, TValue>(expression));
            var propertyToStringCall = Expression
                .Call(nullableProperty, nameof(object.ToString), Type.EmptyTypes);
            var propertyNullCheck = GenerateNullCheck(nullableProperty, 
                ifNotNull: propertyToStringCall);
            
            var cachedObject = expression.Parameters.Single();
            var cachedObjectNullCheck = GenerateNullCheck(cachedObject,
                ifNotNull: propertyNullCheck);

            return Expression.Lambda<Func<T, string>>(cachedObjectNullCheck, cachedObject);
        }

        private MemberExpression GetPropertyExpression<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            switch (expression.Body)
            {
                case MemberExpression property when property.Member.MemberType == MemberTypes.Property:
                    return property;
                default:
                    throw new ArgumentException("Expression should be a single property expression");
            }
        }

        private static Expression GenerateNullCheck(Expression expression, Expression ifNotNull)
            => Expression.Condition(
                test: Expression.Equal(expression, Expression.Constant(null)),
                ifTrue: Expression.Constant(null, typeof(string)),
                ifFalse: ifNotNull);
        
        private static Expression ConvertToNullableExpression(Expression expression)
        {
            var isNullable = !expression.Type.IsValueType 
                             || Nullable.GetUnderlyingType(expression.Type) != null;

            if (isNullable)
            {
                return expression;
            }

            return Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));
        }
    }
}
