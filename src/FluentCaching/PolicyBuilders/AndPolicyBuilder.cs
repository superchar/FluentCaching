namespace FluentCaching.PolicyBuilders
{
    public class AndPolicyBuilder<TValue>
    {
        private readonly TValue _value;

        public AndPolicyBuilder(TValue value)
        {
            _value = value;
        }

        public TValue And() => _value;
    }
}
