using System.Collections.Generic;
using System.Linq;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders
{
    internal class KeyContextBuilder : IKeyContextBuilder
    {
        private static readonly Dictionary<string, object> EmptyContext = new ();

        private readonly Dictionary<string, bool> _keys = new (); // Guaranteed to be thread safe when readonly (unlike hashset)

        private readonly IComplexKeysHelper _complexKeysHelper;
        
        public KeyContextBuilder(IComplexKeysHelper complexKeysHelper)
        {
            _complexKeysHelper = complexKeysHelper;
        }

        public void AddKey(string key) => _keys[key] = true;

        public IDictionary<string, object> BuildKeyContextFromObject(object targetObject)
        {
            var properties = _complexKeysHelper.GetProperties(targetObject.GetType())
                .Where(_ => _keys.ContainsKey(_.Name))
                .ToList();

            if (properties.Count != _keys.Count)
            {
                throw new KeyPartMissingException();
            }

            return properties
                .ToDictionary(p => p.Name, p => p.Get(targetObject));
        }

        public IDictionary<string, object> BuildKeyContextFromString(string targetString)
        {
            if (_keys.Count > 1)
            {
                throw new KeyPartMissingException();
            }

            if (!_keys.Any())
            {
                return EmptyContext;
            }

            return new Dictionary<string, object>
            {
                {
                    _keys.Keys.First(), targetString // First will work faster as _keys is guaranteed to have a single item
                } 
            };
        }
    }
}