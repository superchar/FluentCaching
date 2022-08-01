using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentCaching.Keys.Helpers.Visitors;

namespace FluentCaching.Keys.Helpers
{
    internal class ExpressionsHelper : IExpressionsHelper
    {
        public IReadOnlyCollection<string> GetParameterPropertyNames<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var visitor = new CollectParameterPropertyNamesVisitor();
            visitor.Visit(expression.Body);

            return visitor.Properties;
        }

        public Expression<Func<T, string>> ReplaceResultTypeWithString<T, TValue>(Expression<Func<T, TValue>> expression)
        {
            var body = ConvertToNullableExpression(expression.Body);
            var propertyToStringCall = Expression
                .Call(body, nameof(ToString), Type.EmptyTypes);

            var resultNullCheck = GenerateNullCheck(body,
                ifNotNull: propertyToStringCall);
            var cachedObject = expression.Parameters.Single();
            var cachedObjectNullCheck = GenerateNullCheck(cachedObject,
                ifNotNull: resultNullCheck);

            return Expression.Lambda<Func<T, string>>(cachedObjectNullCheck, cachedObject);
        }

        public Expression<Func<Dictionary<string, object>, string>> ReplaceParameterWithDictionary<T>(Expression<Func<T, string>> expression)
        {
            var dictionaryParam = Expression.Parameter(typeof(Dictionary<string, object>));

            var body = new ReplaceParameterWithDictionaryVisitor(dictionaryParam).Visit(expression.Body);
            
            return Expression.Lambda<Func<Dictionary<string, object>, string>>(body, dictionaryParam);
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
