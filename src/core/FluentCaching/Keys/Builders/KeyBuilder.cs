using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentCaching.Keys.Builders.KeyParts;
using FluentCaching.Keys.Builders.KeyParts.Factories;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders
{
    internal class KeyBuilder : IKeyBuilder
    {
        private const string KeyPartSeparator = ":";
        
        private readonly List<IKeyPartBuilder> _keyPartBuilders = new();

        private readonly IExpressionsHelper _expressionHelper;
        private readonly IKeyContextBuilder _keyContextBuilder;
        private readonly IKeyPartBuilderFactory _keyPartBuilderFactory;

        public KeyBuilder() 
            : this(new KeyContextBuilder(new ExpressionsHelper()), 
                new ExpressionsHelper(),
                new KeyPartBuilderFactory(new ExpressionsHelper()))
        {
        }

        internal KeyBuilder(IKeyContextBuilder keyContextBuilder, 
            IExpressionsHelper expressionHelper,
            IKeyPartBuilderFactory keyPartBuilderFactory)
        {
            _keyContextBuilder = keyContextBuilder;
            _expressionHelper = expressionHelper;
            _keyPartBuilderFactory = keyPartBuilderFactory;
        }

        private bool HasDynamicParts => _keyPartBuilders.Any(_ => _.IsDynamic);
        
        public void AppendStatic<TValue>(TValue value)
            => _keyPartBuilders.Add(_keyPartBuilderFactory.Create(value));
        
        public void AppendExpression<T, TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            foreach (var propertyName in _expressionHelper.GetParameterPropertyNames(valueGetter))
            {
                _keyContextBuilder.AddKey(propertyName);
            }
            
            _keyPartBuilders.Add(_keyPartBuilderFactory.Create(valueGetter));
        }

        public string BuildFromScalarKey(object scalarKey)
        {
            var context = _keyContextBuilder.BuildRetrieveContextFromScalarKey(scalarKey);

            return Build(context);
        }

        public string BuildFromComplexKey(object complexKey)
        {
            var context = _keyContextBuilder.BuildRetrieveContextFromComplexKey(complexKey);

            return Build(context);
            
        }

        public string BuildFromStaticKey()
        {
            if (HasDynamicParts)
            {
                throw new KeyPartMissingException();
            }

            return Build(KeyContext.Null);
        }

        public string BuildFromCachedObject(object cachedObject)
        {
            var context = _keyContextBuilder.BuildCacheContext(cachedObject);

            return Build(context);
        }
        
        private string Build(KeyContext context) 
            => string.Join(string.Empty, BuildKeyParts(context));

        private IEnumerable<string> BuildKeyParts(KeyContext context)
        {
            for (var i = 0; i < _keyPartBuilders.Count; i++)
            {
                yield return _keyPartBuilders[i].Build(context);
                if (i < _keyPartBuilders.Count - 1)
                {
                    yield return KeyPartSeparator;
                }
            }
        }
    }
}
