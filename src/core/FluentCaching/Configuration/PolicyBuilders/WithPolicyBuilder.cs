namespace FluentCaching.Configuration.PolicyBuilders
{
    public class WithPolicyBuilder<TValue>
    {
        private readonly TValue _value;

        public WithPolicyBuilder(TValue value)
        {
            _value = value;
        }

        public TValue With() => _value;
    }
}

