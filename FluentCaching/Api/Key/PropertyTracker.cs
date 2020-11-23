
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace FluentCaching.Api.Key
{
    internal class PropertyTracker
    {
        private const string Self = nameof(Self);

        private readonly HashSet<string> _keys = new HashSet<string>();

        private PropertyTracker()
        {

        }

        public IEnumerable<string> Keys => _keys;

        public static PropertyTracker Create(params object[] sources)
        {
            if (sources.Any(_ => _ != null))
            {
                return new PropertyTracker();
            }

            return EmptyPropertyTracker.Instance;
        }

        public virtual void TrackSelf()
        {
            _keys.Add(Self);
        }

        public virtual void TrackProperty<T, TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            var name = ((MemberExpression)valueGetter.Body).Member.Name;
            _keys.Add(name);
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
