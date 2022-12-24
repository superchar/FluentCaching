using System.Collections.Generic;
using System.Linq;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders
{
    internal class KeyContextBuilder : IKeyContextBuilder
    {
        private static readonly KeyContext EmptyRetrieveContext = new (new Dictionary<string, object>());

        private readonly Dictionary<string, bool> _keys = new (); // Guaranteed to be thread safe when readonly (unlike hashset)

        private readonly IExpressionsHelper _expressionsHelper;
        
        public KeyContextBuilder(IExpressionsHelper expressionsHelper)
        {
            _expressionsHelper = expressionsHelper;
        }

        public void AddKey(string key)
        {
            if (_keys.ContainsKey(key))
            {
                throw new KeyPartException(
                    $"Property name duplicates are not supported. Duplicated property name - {key}.");
            }
            
            _keys[key] = true;
        }

        public KeyContext BuildRetrieveContextFromComplexKey(object complexKey)
        {
            var properties = _expressionsHelper.GetProperties(complexKey.GetType())
                .Where(_ => _keys.ContainsKey(_.Name))
                .ToList();

            if (properties.Count != _keys.Count)
            {
                throw new KeyPartMissingException();
            }

            var retrieveContext = properties
                .ToDictionary(p => p.Name, p => p.Get(complexKey));
            return new KeyContext(retrieveContext);
        }

        public KeyContext BuildRetrieveContextFromScalarKey(object scalarKey)
        {
            if (_keys.Count > 1)
            {
                throw new KeyPartMissingException();
            }

            if (!_keys.Any())
            {
                return EmptyRetrieveContext;
            }

            var retrieveContext = new Dictionary<string, object>
            {
                {
                    _keys.Keys.First(), scalarKey // First will work faster as _keys is guaranteed to have a single item
                } 
            };

            return new KeyContext(retrieveContext);
        }

        public KeyContext BuildCacheContext(object cachedObject) => new(cachedObject);
    }
}