using FluentCaching.Api.Ttl;
using FluentCaching.Keys;
using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class CachingOptionsBuilder
    {
        private readonly CachingOptions _currentOptions;

        internal CachingOptionsBuilder(ITracksProperties propertyTracker)
        {
            _currentOptions.PropertyTracker = propertyTracker;
        }

        public TtlBuilder WithTtlOf(short value) => new TtlBuilder(_currentOptions, value);
    }
}
