
using System;
using System.Threading.Tasks;
using FluentCaching.Configuration;
using FluentCaching.Parameters;

namespace FluentCaching.Api
{
    public class ExpirationBuilder
    {
        private CachingOptions _currentOptions;

        public ExpirationBuilder(CachingOptions currentOptions)
        {
            _currentOptions = currentOptions;
        }

        public CachingOptions CachingOptions => _currentOptions;

        public ExpirationBuilder ExpirationType(ExpirationType expirationType)
        {
            _currentOptions.ExpirationType = expirationType;

            return this;
        }

        public ExpirationBuilder AbsoluteExpiration()
        {
            return ExpirationType(Parameters.ExpirationType.Absolute);
        }

        public ExpirationBuilder SlidingExpiration()
        {
            return ExpirationType(Parameters.ExpirationType.Sliding);
        }
    }
}
