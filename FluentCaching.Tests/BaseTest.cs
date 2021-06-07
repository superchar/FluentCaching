using System;
using System.Collections.Generic;
using FluentCaching.Configuration;
using FluentCaching.Tests.Mocks;

namespace FluentCaching.Tests
{
    public class BaseTest : IDisposable
    {
        protected DictionaryCacheImplementation Cache { get; set; } = new DictionaryCacheImplementation();

        protected CachingConfiguration Configuration { get; set; } = CachingConfiguration.Create();

        protected Dictionary<string, object> Dictionary => Cache.Dictionary;

        protected BaseTest()
        {
            Configuration.SetImplementation(Cache);
        }

        public void Dispose()
        {
            Configuration.Reset();
        }
    }
}
