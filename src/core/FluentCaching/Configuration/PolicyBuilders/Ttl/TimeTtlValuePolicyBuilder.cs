namespace FluentCaching.Configuration.PolicyBuilders.Ttl
{
    public class TimeTtlValuePolicyBuilder
    {
        private readonly TimeTtlPolicyBuilder _ttlPolicyBuilder;

        public TimeTtlValuePolicyBuilder(TimeTtlPolicyBuilder ttlPolicyBuilder)
        {
            _ttlPolicyBuilder = ttlPolicyBuilder;
        }

        public TimeTtlPolicyBuilder And(ushort value)
        {
            _ttlPolicyBuilder.SetCurrentValue(value);
            return _ttlPolicyBuilder;
        }

        public ExpirationTypePolicyBuilder With() => _ttlPolicyBuilder.Build();
    }
}
