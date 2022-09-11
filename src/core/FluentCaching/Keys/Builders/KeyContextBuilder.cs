using System.Collections.Generic;
using System.Linq;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders
{
    internal class KeyContextBuilder : IKeyContextBuilder
    {
        private static readonly KeyContext EmptyRetrieveContext = new (new Dictionary<string, object>());

        private readonly Dictionary<string, bool> _keys = new (); // Guaranteed to be thread safe when readonly (unlike hashset)

        private readonly IComplexKeysHelper _complexKeysHelper;
        
        public KeyContextBuilder(IComplexKeysHelper complexKeysHelper)
        {
            _complexKeysHelper = complexKeysHelper;
        }

        public void AddKey(string key) => _keys[key] = true;

        public KeyContext BuildRetrieveContextFromComplexKey(object complexKey)
        {
            var properties = _complexKeysHelper.GetProperties(complexKey.GetType())
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