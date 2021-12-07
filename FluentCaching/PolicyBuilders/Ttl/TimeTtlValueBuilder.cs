namespace FluentCaching.PolicyBuilders.Ttl
{
    public class TimeTtlValueBuilder
    {
        private readonly TimeTtlBuilder _ttlBuilder;

        public TimeTtlValueBuilder(TimeTtlBuilder ttlBuilder)
        {
            _ttlBuilder = ttlBuilder;
        }

        public TimeTtlBuilder And(short value)
        {
            _ttlBuilder.SetCurrentValue(value);
            return _ttlBuilder;
        }

        public ExpirationTypeBuilder And() => _ttlBuilder.Build();
    }
}
