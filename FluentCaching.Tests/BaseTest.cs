

using System;
using System.Collections.Generic;
using FluentCaching.Configuration;
using FluentCaching.Tests.Mocks;

namespace FluentCaching.Tests
{
    public class BaseTest : IDisposable
    {
        protected DictionaryCacheImplementation Cache { get; set; } = new DictionaryCacheImplementation();

        protected Dictionary<string, object> Dictionary => Cache.Dictionary;

        public BaseTest()
        {
            CachingConfiguration.Instance.SetImplementation(Cache);
        }

        public void Dispose()
        {
            CachingConfiguration.Instance.Reset();
        }
    }
}
