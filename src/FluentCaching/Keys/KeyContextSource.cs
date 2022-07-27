using System.Collections.Generic;

namespace FluentCaching.Keys
{
    public struct KeyContextSource<T>
        where T : class
    {
        public static readonly KeyContextSource<T> Null = new(null, null);

        public KeyContextSource(T store) : this(store, null)
        {
        }
        
        public KeyContextSource(IDictionary<string, object> keyContext) : this(null, keyContext)
        {
        }

        private KeyContextSource(T store, IDictionary<string, object> retrieve)
        {
            Store = store;
            Retrieve = retrieve;
        }

        public T Store { get; }

        public IDictionary<string, object> Retrieve { get; }
    }
}