

namespace FluentCaching.Api.Ttl
{
    public class TtlCurrentValueBuilder
    {
        private readonly TtlBuilder _ttlBuilder;

        public TtlCurrentValueBuilder(TtlBuilder ttlBuilder)
        {
            _ttlBuilder = ttlBuilder;
        }

        public TtlBuilder And(short value)
        {
            _ttlBuilder.SetCurrentValue(value);
            return _ttlBuilder;
        }

        public ExpirationBuilder And()
        {
            return _ttlBuilder.Build();
        }
    }
}
