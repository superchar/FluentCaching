using FluentCaching.Cache.Models;
using System;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class TimeTtlBuilder
    {
        private CacheOptions _currentOptions;

        private ushort _currentValue;

        private ushort _seconds;

        private ushort _minutes;

        private ushort _hours;

        private ushort _days;

        public TimeTtlBuilder(CacheOptions currentOptions, ushort currentValue)
        {
            _currentOptions = currentOptions;
            _currentValue = currentValue;
        }

        public TimeTtlValueBuilder Seconds
        {
            get
            {
                _seconds = _currentValue;
                return new TimeTtlValueBuilder(this);
            }
        }

        public TimeTtlValueBuilder Minutes
        {
            get
            {
                _minutes = _currentValue;
                return new TimeTtlValueBuilder(this);
            }
        }

        public TimeTtlValueBuilder Hours
        {
            get
            {
                _hours = _currentValue;
                return new TimeTtlValueBuilder(this);
            }
        }

        public TimeTtlValueBuilder Days
        {
            get
            {
                _days = _currentValue;
                return new TimeTtlValueBuilder(this);
            }
        }

        internal void SetCurrentValue(ushort value) => _currentValue = value;

        internal ExpirationTypeBuilder Build()
        {
            _currentOptions.Ttl = new TimeSpan(_days, _hours, _minutes, _seconds);
            return new ExpirationTypeBuilder(_currentOptions);
        }
    }
}
