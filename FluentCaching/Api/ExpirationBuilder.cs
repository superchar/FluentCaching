
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

        public StoringHelperWrapper ExpirationType(ExpirationType expirationType)
        {
            _currentOptions.ExpirationType = expirationType;
            return new StoringHelperWrapper(_currentOptions);
        }

        public StoringHelperWrapper AbsoluteExpiration()
        {
            return ExpirationType(Parameters.ExpirationType.Absolute);
        }

        public StoringHelperWrapper SlidingExpiration()
        {
            return ExpirationType(Parameters.ExpirationType.Sliding);
        }
    }
}
