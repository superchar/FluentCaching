using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Builders.KeyParts.Factories;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders
{
    internal class KeyBuilder<T> : IKeyBuilder<T>
        where T : class
    {
        private List<IKeyPartBuilder<T>> _keyPartBuilders = new();

        private readonly IExpressionsHelper _expressionHelper;
        private readonly IKeyContextBuilder<T> _keyContextBuilder;
        private readonly IKeyPartBuilderFactory<T> _keyPartBuilderFactory;

        public KeyBuilder() 
            : this(new KeyContextBuilder<T>(new ComplexKeysHelper()), 
                new ExpressionsHelper(),
                new KeyPartBuilderFactory<T>(new ExpressionsHelper()))
        {
        }

        internal KeyBuilder(IKeyContextBuilder<T> keyContextBuilder, 
            IExpressionsHelper expressionHelper,
            IKeyPartBuilderFactory<T> keyPartBuilderFactory)
        {
            _keyContextBuilder = keyContextBuilder;
            _expressionHelper = expressionHelper;
            _keyPartBuilderFactory = keyPartBuilderFactory;
        }

        private bool HasDynamicParts => _keyPartBuilders.Any(_ => _.IsDynamic);
        
        public void AppendStatic<TValue>(TValue value)
            => _keyPartBuilders.Add(_keyPartBuilderFactory.Create(value));
        
        public void AppendExpression<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            var property = _expressionHelper.GetPropertyName(valueGetter);
            _keyContextBuilder.AddKey(property);
            _keyPartBuilders.Add(_keyPartBuilderFactory.Create(valueGetter));
        }

        public string BuildFromStringKey(string stringKey)
        {
            var context = _keyContextBuilder.BuildRetrieveContextFromStringKey(stringKey);

            return Build(context);
        }

        public string BuildFromObjectKey(object objectKey)
        {
            var context = _keyContextBuilder.BuildRetrieveContextFromObjectKey(objectKey);

            return Build(context);
            
        }

        public string BuildFromStaticKey()
        {
            if (HasDynamicParts)
            {
                throw new KeyPartMissingException();
            }

            return Build(KeyContext<T>.Null);
        }

        public string BuildFromCachedObject(T cachedObject)
        {
            var context = _keyContextBuilder.BuildCacheContext(cachedObject);

            return Build(context);
        }
        
        private string Build(KeyContext<T> context) => string.Join(string.Empty, BuildKeyParts(context));

        private IEnumerable<string> BuildKeyParts(KeyContext<T> context)
        {
            foreach (var builder in _keyPartBuilders)
            {
                yield return builder.Build(context);
            }
        }
    }
}
