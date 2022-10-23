using FluentCaching.Cache;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;

namespace FluentCaching.Tests.Integration.Cache
{
    public class CacheOperationBaseTest : BaseTest
    {
        protected const string Key = "user";

        protected CacheOperationBaseTest()
        {
            Cache = CacheBuilder
                .For<User>(u => u.UseAsKey(Key).Complete())
                .Build();
        }

        protected ICache Cache { get; }

    }
}
