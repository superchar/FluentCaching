using System.Collections.Generic;
using System.Linq;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders
{
    internal class KeyContextBuilder<T> : IKeyContextBuilder<T>
        where T : class
    {
        private static readonly KeyContext<T> EmptyRetrieveContext 
            = new (new Dictionary<string, object>());

        private readonly Dictionary<string, bool> _keys = new (); // Guaranteed to be thread safe when readonly (unlike hashset)

        private readonly IComplexKeysHelper _complexKeysHelper;
        
        public KeyContextBuilder(IComplexKeysHelper complexKeysHelper)
        {
            _complexKeysHelper = complexKeysHelper;
        }

        public void AddKey(string key) => _keys[key] = true;

        public KeyContext<T> BuildRetrieveContextFromObjectKey(object targetObject)
        {
            var properties = _complexKeysHelper.GetProperties(targetObject.GetType())
                .Where(_ => _keys.ContainsKey(_.Name))
                .ToList();

            if (properties.Count != _keys.Count)
            {
                throw new KeyPartMissingException();
            }

            var retrieveContext = properties
                .ToDictionary(p => p.Name, p => p.Get(targetObject));
            return new KeyContext<T>(retrieveContext);
        }

        public KeyContext<T> BuildRetrieveContextFromStringKey(string targetString)
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
                    _keys.Keys.First(), targetString // First will work faster as _keys is guaranteed to have a single item
                } 
            };

            return new KeyContext<T>(retrieveContext);
        }

        public KeyContext<T> BuildCacheContext(T cachedObject) => new(cachedObject);
    }
}