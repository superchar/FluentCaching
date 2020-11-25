
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

        private readonly IDictionary<string, bool> _keys = new Dictionary<string, bool>(); // Guaranteed to be thread safe unlike hashset

        private PropertyTracker()
        {

        }

        public static PropertyTracker Create(params object[] sources)
        {
            return sources.Any(_ => _ != null) ? new PropertyTracker() : EmptyPropertyTracker.Instance;
        }

        public IDictionary<string, object> GetValueSourceDictionary(object targetObject)
        {
            return GetValidDescriptors(targetObject)
                .ToDictionary(p => p.Name, p => p.GetValue(targetObject)); 
        }

        public IDictionary<string, object> GetValueSourceDictionary(string targetString)
        {
            if (_keys.Count != 1)
            {
                throw new ArgumentException(nameof(targetString), "A single dynamic key must be defined in configuration");
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

        public virtual void TrackProperty<T, TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            var name = ((MemberExpression)valueGetter.Body).Member.Name;
            _keys[name] = true;
        }

        private IEnumerable<PropertyDescriptor> GetValidDescriptors(object targetObject) //TODO: caching and reflection optimization
        {
            return TypeDescriptor.GetProperties(targetObject)
                .Cast<PropertyDescriptor>()
                .Where(property => _keys.ContainsKey(property.Name));
        }

        private class EmptyPropertyTracker : PropertyTracker
        {
            public static readonly EmptyPropertyTracker Instance = new EmptyPropertyTracker();

            [MethodImpl(MethodImplOptions.AggressiveInlining)] // TODO: Possibly will always be inlined without attrs
            public override void TrackProperty<T, TValue>(Expression<Func<T, TValue>> valueGetter)
            {
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void TrackSelf()
            {
            }
        }
    }
}
