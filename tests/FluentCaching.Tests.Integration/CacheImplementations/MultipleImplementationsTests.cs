using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Models;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Integration.CacheImplementations
{
    public class MultipleImplementationsTests : BaseTest
    {
        private const string Key = "user";

        private readonly Mock<ICacheImplementation> _userSpecificImplementationMock = new Mock<ICacheImplementation>();

        [Fact]
        public async Task CacheAsync_SpecificImplementation_UsesSpecificCache()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(Key).Complete(_userSpecificImplementationMock.Object))
                .Build();

            await cache.CacheAsync(User.Test);

            _userSpecificImplementationMock
                .Verify(i => i.CacheAsync(Key, User.Test, It.IsAny<CacheOptions>()), Times.Once);
            CacheImplementationMock
                .Verify(i => i.CacheAsync(It.IsAny<string>(), It.IsAny<User>(), It.IsAny<CacheOptions>()), Times.Never);
        }

        [Fact]
        public async Task CacheAsync_GenericImplementation_UsesGenericCache()
        {
            var cache = CacheBuilder
                .For<User>(u => u.UseAsKey(Key).Complete())
                .Build();

            await cache.CacheAsync(User.Test);

            CacheImplementationMock
                .Verify(i => i.CacheAsync(Key, User.Test, It.IsAny<CacheOptions>()), Times.Once);

        }
    }
}
