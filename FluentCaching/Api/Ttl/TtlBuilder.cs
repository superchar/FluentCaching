
using System;
using System.Net.Http.Headers;
using FluentCaching.Parameters;

namespace FluentCaching.Api.Ttl
{
    public class TtlBuilder
    {
        private CachingOptions _currentOptions;

        private short _currentValue;

        private short _seconds = 0;

        private short _minutes = 0;

        private short _hours = 0;

        private short _days = 0;

        public TtlBuilder(CachingOptions currentOptions, short currentValue)
        {
            _currentOptions = currentOptions;
            _currentValue = currentValue;
        }

        public TtlCurrentValueBuilder Seconds
        {
            get
            {
                _seconds = _currentValue;
                return new TtlCurrentValueBuilder(this);
            }
        }

        public TtlCurrentValueBuilder Minutes
        {
            get
            {
                _minutes = _currentValue;
                return new TtlCurrentValueBuilder(this);
            }
        }

        public TtlCurrentValueBuilder Hours
        {
            get
            {
                _hours = _currentValue;
                return new TtlCurrentValueBuilder(this);
            }
        }

        public TtlCurrentValueBuilder Days
        {
            get
            {
                _days = _currentValue;
                return new TtlCurrentValueBuilder(this);
            }
        }

        internal void SetCurrentValue(short value) => _currentValue = value;

        internal ExpirationBuilder Build()
        {
            _currentOptions.Ttl = new TimeSpan(_days, _hours, _minutes, _seconds);
            return new ExpirationBuilder(_currentOptions);
        }
    }
}
