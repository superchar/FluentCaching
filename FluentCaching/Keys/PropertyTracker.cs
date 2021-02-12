
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FluentCaching.Exceptions;
using FluentCaching.Keys.Complex;

namespace FluentCaching.Keys
{
    internal class PropertyTracker<T> : ITracksProperties
        where T : class
    {
        private const string Self = nameof(Self);

        private static readonly Func<T, IDictionary<string, object>, string> DefaultFactory =
            (obj, valueDict) => string.Empty;

        private static readonly Dictionary<string, object> EmptyValueSource = new Dictionary<string, object>(0);

        private readonly IDictionary<string, bool>
            _keys = new Dictionary<string, bool>(); // Guaranteed to be thread safe when readonly (unlike hashset)

        private Func<T, IDictionary<string, object>, string> _factory = DefaultFactory;

        public string GetStoreKey(T obj)
        {
            return _factory(obj, null);
        }

        public string GetRetrieveKeySimple(string stringKey)
        {
            var valueSource = GetValueSourceDictionary(stringKey);

            return _factory(null, valueSource);
        }

        public string GetRetrieveKeyComplex(object obj)
        {
            var valueSource = GetValueSourceDictionary(obj);

            return _factory(null, valueSource);
        }

        public void TrackSelf()
        {
            _keys[Self] = true;

            var factory = _factory;

            _factory = (obj, valueDict) => factory(obj, valueDict) + (obj?.ToString() ?? valueDict[Self].ToString());
        }

        public void TrackStatic<TValue>(TValue value)
        {
            var staticPart = value?.ToString();

            ThrowIfKeyPartIsNull(staticPart);

            var factory = _factory;

            _factory = (obj, valueDict) => factory(obj, valueDict) + staticPart;
        }

        public void TrackExpression<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            var property = ExpressionsHelper.GetProperty(valueGetter).Name;

            var compiledExpression = valueGetter.Compile();

            _keys[property] = true;

            var factory = _factory;

            _factory = (obj, valueDict) =>
                factory(obj, valueDict) +
                (obj != null
                    ? ThrowIfKeyPartIsNull(compiledExpression(obj)?.ToString())
                    : valueDict[property].ToString());
        }

        private IDictionary<string, object> GetValueSourceDictionary(object targetObject)
        {
            var properties = ComplexKeysHelper.GetProperties(targetObject.GetType())
                .Where(_ => _keys.ContainsKey(_.Name))
                .ToList();

            if (properties.Count != _keys.Count)
            {
                throw new KeyNotFoundException("Key schema is not correct");
            }

            return properties
                .ToDictionary(p => p.Name, p => p.Get(targetObject));
        }

        private IDictionary<string, object> GetValueSourceDictionary(string targetString)
        {
            if (_keys.Count > 1)
            {
                throw new ArgumentException(
                    "A single dynamic key must be defined in configuration", nameof(targetString));
            }

            if (!_keys.Any())
            {
                return EmptyValueSource;
            }

            return new Dictionary<string, object>
            {
                {
                    _keys.Keys.First(), targetString
                } // First will work faster as _keys is guaranteed to have a single item
            };
        }


        private static string ThrowIfKeyPartIsNull(string part)
        {
            if (part == null)
            {
                throw new KeyPartNullException();
            }

            return part;
        }
    }
}
