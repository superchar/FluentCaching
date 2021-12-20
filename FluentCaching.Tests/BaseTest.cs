using System.Collections.Generic;
using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using FluentCaching.Tests.Integration.Fakes;

namespace FluentCaching.Tests.Integration
{
    public class BaseTest
    {
        protected BaseTest()
        {
            CacheBuilder.SetGenericCache(DictionaryCacheImplementation);
        }

        protected ICacheBuilder CacheBuilder { get; set; } = new CacheBuilder();

        protected DictionaryCacheImplementation DictionaryCacheImplementation { get; set; } = new DictionaryCacheImplementation();

        protected Dictionary<string, object> Dictionary => DictionaryCacheImplementation.Dictionary;
    }
}
