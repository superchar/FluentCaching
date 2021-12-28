namespace FluentCaching.PolicyBuilders
{
    public class AndBuilder<TValue>
    {
        private readonly TValue _value;

        public AndBuilder(TValue value)
        {
            _value = value;
        }

        public TValue And() => _value;
    }
}
