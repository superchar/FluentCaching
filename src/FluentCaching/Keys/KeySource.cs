using System.Collections.Generic;

namespace FluentCaching.Keys
{
    public struct KeySource<T>
        where T : class
    {
        public static readonly KeySource<T> Null = new(null, null);

        public KeySource(T cachedObject) : this(cachedObject, null)
        {
        }
        
        public KeySource(IDictionary<string, object> keyContext) : this(null, keyContext)
        {
        }

        private KeySource(T cachedObject, IDictionary<string, object> keyContext)
        {
            CachedObject = cachedObject;
            KeyContext = keyContext;
        }

        public T CachedObject { get; }

        public IDictionary<string, object> KeyContext { get; }
    }
}