using System.Collections.Generic;

namespace FluentCaching.Keys.Models
{
    public struct KeyContext<T>
        where T : class
    {
        public static readonly KeyContext<T> Null = new(null, null);

        public KeyContext(T store) : this(store, null)
        {
        }
        
        public KeyContext(IDictionary<string, object> keyContext) : this(null, keyContext)
        {
        }

        private KeyContext(T store, IDictionary<string, object> retrieve)
        {
            Store = store;
            Retrieve = retrieve;
        }

        public T Store { get; }

        public IDictionary<string, object> Retrieve { get; }
    }
}