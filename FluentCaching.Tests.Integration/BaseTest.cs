using FluentCaching.Cache;
using FluentCaching.Cache.Builders;
using Moq;

namespace FluentCaching.Tests.Integration
{
    public class BaseTest
    {
        protected BaseTest()
        {
            CacheBuilder.SetGenericCache(CacheImplementationMock.Object);
        }

        protected ICacheBuilder CacheBuilder { get; set; } = new CacheBuilder();

        protected Mock<ICacheImplementation> CacheImplementationMock { get; set; } = new Mock<ICacheImplementation>();
    }
}
