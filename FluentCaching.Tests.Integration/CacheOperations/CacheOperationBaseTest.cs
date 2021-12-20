using FluentCaching.Cache;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;

namespace FluentCaching.Tests.Integration.CacheOperations
{
    public class CacheOperationBaseTest : BaseTest
    {
        protected const string KEY = "user";

        public CacheOperationBaseTest()
        {
            Cache = CacheBuilder
                .For<User>(u => u.UseAsKey(KEY).Complete())
                .Build();
        }

        protected ICache Cache { get; }

    }
}
