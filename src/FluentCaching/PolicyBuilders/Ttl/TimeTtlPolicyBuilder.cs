using FluentCaching.Cache.Models;
using System;

namespace FluentCaching.PolicyBuilders.Ttl
{
    public class TimeTtlPolicyBuilder
    {
        private CacheOptions _currentOptions;

        private ushort _currentValue;

        private ushort _seconds;

        private ushort _minutes;

        private ushort _hours;

        private ushort _days;

        public TimeTtlPolicyBuilder(CacheOptions currentOptions, ushort currentValue)
        {
            _currentOptions = currentOptions;
            _currentValue = currentValue;
        }

        public TimeTtlValuePolicyBuilder Seconds
        {
            get
            {
                _seconds = _currentValue;
                return new TimeTtlValuePolicyBuilder(this);
            }
        }

        public TimeTtlValuePolicyBuilder Minutes
        {
            get
            {
                _minutes = _currentValue;
                return new TimeTtlValuePolicyBuilder(this);
            }
        }

        public TimeTtlValuePolicyBuilder Hours
        {
            get
            {
                _hours = _currentValue;
                return new TimeTtlValuePolicyBuilder(this);
            }
        }

        public TimeTtlValuePolicyBuilder Days
        {
            get
            {
                _days = _currentValue;
                return new TimeTtlValuePolicyBuilder(this);
            }
        }

        internal void SetCurrentValue(ushort value) => _currentValue = value;

        internal ExpirationTypePolicyBuilder Build()
        {
            _currentOptions.Ttl = new TimeSpan(_days, _hours, _minutes, _seconds);
            return new ExpirationTypePolicyBuilder(_currentOptions);
        }
    }
}
