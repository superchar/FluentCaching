using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentCaching.Keys;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders
{
    internal class KeyBuilder<T> : IKeyBuilder<T>
        where T : class
    {
        private bool _hasDynamicParts = false;
        private List<Func<KeyContext<T>, string>> _keyPartBuilders = new();

        private readonly IKeyContextBuilder<T> _keyContextBuilder;
        private readonly IExpressionsHelper _expressionHelper;

        public KeyBuilder() 
            : this(new KeyContextBuilder<T>(new ComplexKeysHelper()), new ExpressionsHelper())
        {
            
        }

        internal KeyBuilder(IKeyContextBuilder<T> keyContextBuilder, 
            IExpressionsHelper expressionHelper)
        {
            _keyContextBuilder = keyContextBuilder;
            _expressionHelper = expressionHelper;
        }

        public void AppendStatic<TValue>(TValue value)
        {
            var staticPart = value?.ToString();
            ThrowIfKeyPartIsEmpty(staticPart);
            _keyPartBuilders.Add(_ => staticPart);
        }

        public void AppendExpression<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _hasDynamicParts = true;
            var property = _expressionHelper.GetProperty(valueGetter).Name;
            _keyContextBuilder.AddKey(property);
            var compiledExpression = _expressionHelper
                .RewriteWithSafeToString(valueGetter)
                .Compile();
            Func<KeyContext<T>, string> keyPartBuilder = 
                _ => ThrowIfKeyPartIsEmpty(_.Store != null
                    ? compiledExpression(_.Store)
                    : _.Retrieve[property]?.ToString());
            _keyPartBuilders.Add(keyPartBuilder);
        }

        public string BuildFromCachedObject(T cachedObject)
        {
            var context = _keyContextBuilder.BuildCacheContext(cachedObject);
            
            return BuildKey(context);
        }

        public string BuildFromStringKey(string stringKey)
        {
            var context = _keyContextBuilder.BuildRetrieveContextFromStringKey(stringKey);
            
            return BuildKey(context);
        }

        public string BuildFromObjectKey(object objectKey)
        {
            var context = _keyContextBuilder.BuildRetrieveContextFromObjectKey(objectKey);
            
            return BuildKey(context);
        }

        public string BuildFromStaticKey()
        {
            if (_hasDynamicParts)
            {
                throw new KeyPartMissingException();
            }

            return BuildKey(KeyContext<T>.Null);
        }

        private static string ThrowIfKeyPartIsEmpty(string part)
        {
            if (string.IsNullOrEmpty(part))
            {
                throw new KeyPartMissingException();
            }

            return part;
        }

        private string BuildKey(KeyContext<T> context) => string.Join(string.Empty, BuildKeyParts(context));
        
        private IEnumerable<string> BuildKeyParts(KeyContext<T> context)
        {
            foreach (var action in _keyPartBuilders)
            {
                yield return action(context);
            }
        }
    }
}
