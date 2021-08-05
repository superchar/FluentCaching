using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Tests.Extensions;
using FluentCaching.Tests.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Cache
{
    public class RetrieveCachedTests : BaseTest
    {
        private Mock<IUserRetriever> _userRetrieverMock;

        public RetrieveCachedTests()
        {
            _userRetrieverMock = new Mock<IUserRetriever>();
        }

        [Fact]
        public async Task RetrieveAsync_SimpleKeyValueIsCached_DoesntCallRetriver()
        {
            const string key = "user";

            Configuration
                .For<User>(u => u.UseAsKey(key).Complete());

            await User.Test.CacheAsync(Configuration);

            var result = await key.RetrieveAsync<User>(Configuration)
                .Or()
                .CacheAsync(() => _userRetrieverMock.Object.GetUser("user"), Configuration);

            result.Should().Be(User.Test);

            _userRetrieverMock
                .Verify(_ => _.GetUser(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task RetrieveAsync_SimpleKeyValueIsNotCached_CallsRetriever()
        {
            const string key = "user";

            Configuration
                .For<User>(u => u.UseAsKey(key).Complete());

            _userRetrieverMock
                .Setup(_ => _.GetUser("user"))
                .ReturnsAsync(User.Test);

            var result = await key.RetrieveAsync<User>(Configuration)
                .Or()
                .CacheAsync(() => _userRetrieverMock.Object.GetUser("user"), Configuration);

            result.Should().Be(User.Test);

            _userRetrieverMock
                .Verify(_ => _.GetUser("user"), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsync_ComplexKeyValueIsCached_DoesntCallRetriever()
        {
            Configuration
                .For<User>(u => u.UseAsKey(_ => _.LastName).CombinedWith(_ => _.Id).Complete());

            await User.Test.CacheAsync(Configuration);

            var key = new { LastName = "Doe", Id = 1 };

            var result = await key.RetrieveAsync<User>(Configuration)
                .Or()
                .CacheAsync(() => _userRetrieverMock.Object.GetUser(key.Id), Configuration);

            result.Should().Be(User.Test);

            _userRetrieverMock
                .Verify(_ => _.GetUser(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RetrieveAsync_ComplexKeyValueIsNotCached_CachesValue()
        {
            Configuration
                .For<User>(u => u.UseAsKey(_ => _.LastName).CombinedWith(_ => _.Id).Complete());

            _userRetrieverMock
                .Setup(_ => _.GetUser(1))
                .ReturnsAsync(User.Test);

            var key = new { LastName = "Doe", Id = 1 };

            var result = await key.RetrieveAsync<User>(Configuration)
                .Or()
                .CacheAsync(() => _userRetrieverMock.Object.GetUser(key.Id), Configuration);

            result.Should().Be(User.Test);

            _userRetrieverMock
                .Verify(_ => _.GetUser(1), Times.Once);
        }
    }

    public interface IUserRetriever
    {
        Task<User> GetUser(string key);

        Task<User> GetUser(int key);
    }
}
