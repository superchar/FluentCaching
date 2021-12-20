using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using FluentCaching.Tests.Integration.Models;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Cache;
using Moq;
using FluentCaching.Cache.Models;

namespace FluentCaching.Tests.Integration.CacheImplementations
{
    public class MultipleImplementationsTests : BaseTest
    {
        private const string KEY = "user";

        private readonly Mock<ICacheImplementation> _userSpecificImplementationMock = new Mock<ICacheImplementation>();

        [Fact]
        public async Task CacheAsync_SpecificImplementation_UsesSpecificCache()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(KEY).Complete(_userSpecificImplementationMock.Object))
                .Build();

            await cache.CacheAsync(User.Test);

            _userSpecificImplementationMock
                .Verify(i => i.CacheAsync(KEY, User.Test, It.IsAny<CacheOptions>()), Times.Once);
            CacheImplementationMock
                .Verify(i => i.CacheAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<CacheOptions>()), Times.Never);
        }

        [Fact]
        public async Task CacheAsync_GenericImplementation_UsesGenericCache()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(KEY).Complete())
                .Build();

            await cache.CacheAsync(User.Test);

            CacheImplementationMock
                .Verify(i => i.CacheAsync(KEY, User.Test, It.IsAny<CacheOptions>()), Times.Once);

        }
    }
}
