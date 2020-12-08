
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FluentCaching.Keys
{
    internal class PropertyTracker
    {
        private const string Self = nameof(Self);

        private static readonly Dictionary<string, object> EmptyValueSource = new Dictionary<string, object>(0);

        private readonly IDictionary<string, bool> _keys = new Dictionary<string, bool>(); // Guaranteed to be thread safe when readonly (unlike hashset)

        private PropertyTracker()
        {

        }

        public static PropertyTracker Create(params object[] sources)
        {
            return sources.Any(_ => _ != null) ? EmptyPropertyTracker.Instance : new PropertyTracker();
        }

        public IDictionary<string, object> GetValueSourceDictionary(object targetObject)
        {
            var descriptors = GetValidDescriptors(targetObject);

            if (descriptors.Count != _keys.Count)
            {
                throw new KeyNotFoundException("Key schema is not correct");
            }

            return descriptors
                .ToDictionary(p => p.Name, p => p.GetValue(targetObject)); 
        }

        public IDictionary<string, object> GetValueSourceDictionary(string targetString)
        {
            if (_keys.Count > 1)
            {
                throw new ArgumentException(nameof(targetString), "A single dynamic key must be defined in configuration");
            }

            if (!_keys.Any())
            {
                return EmptyValueSource;
            }

            return new Dictionary<string, object>
            {
                {_keys.Keys.Single(), targetString}
            };
        }

        public virtual void TrackSelf()
        {
            _keys[Self] = true;
        }

        public virtual void TrackExpression<T, TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            var name = ExpressionsHelper.GetProperty(valueGetter).Name;
            _keys[name] = true;
        }

        private List<PropertyDescriptor> GetValidDescriptors(object targetObject) //TODO: caching and reflection optimization
        {
            return TypeDescriptor.GetProperties(targetObject)
                .Cast<PropertyDescriptor>()
                .Where(property => _keys.ContainsKey(property.Name))
                .ToList();
        }

        private class EmptyPropertyTracker : PropertyTracker
        {
            public static readonly EmptyPropertyTracker Instance = new EmptyPropertyTracker();

            public override void TrackExpression<T, TValue>(Expression<Func<T, TValue>> valueGetter)
            {
            }

            public override void TrackSelf()
            {
            }
        }
    }
}
