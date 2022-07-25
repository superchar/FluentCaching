using System.Threading.Tasks;
using FluentAssertions;
using FluentCaching.Cache;
using FluentCaching.Cache.Helpers;
using FluentCaching.Cache.Models;
using FluentCaching.Configuration;
using FluentCaching.Configuration.Exceptions;
using FluentCaching.Keys;
using FluentCaching.Keys.Builders;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache
{
    public class CacheServiceTests
    {
        private readonly Mock<ICacheConfiguration> _cacheConfigurationMock;
        private readonly Mock<IConcurrencyHelper> _concurrencyHelperMock;

        private readonly CacheService _sut;

        public CacheServiceTests()
        {
            _cacheConfigurationMock = new Mock<ICacheConfiguration>();
            _concurrencyHelperMock = new Mock<IConcurrencyHelper>();

            _sut = new CacheService(_cacheConfigurationMock.Object, _concurrencyHelperMock.Object);
        }

        [Fact]
        public async Task CacheAsync_MissingCachingPolicy_ThrowsException()
        {
            await _sut.Invoking(s => s.CacheAsync(new User()))
                .Should()
                .ThrowAsync<ConfigurationNotFoundException>()
                .WithMessage("Caching configuration for type 'FluentCaching.Tests.Unit.Models.User' is not found");
        }

        [Fact]
        public async Task CacheAsync_MissingCacheImplementation_ThrowsException()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options)
                .Returns(new CacheOptions());
            var keyBuilderMock = new Mock<IKeyBuilder<User>>();
            keyBuilderMock
                .Setup(p => p.BuildFromCachedObject(It.Is<User>(u => u.Name == "Test name")))
                .Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder)
                .Returns(keyBuilderMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>())
                .Returns(configurationItemMock.Object);

            var user = new User { Name = "Test name" };
            await _sut.Invoking(s => s.CacheAsync(user)).Should()
                .ThrowAsync<CacheImplementationNotFoundException>()
                .WithMessage("No caching implementation configured for type - 'FluentCaching.Tests.Unit.Models.User'");
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            keyBuilderMock
                .Verify(p => p.BuildFromCachedObject(It.Is<User>(u => u.Name == "Test name")), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task CacheAsync_SpecificCacheImplementation_UsesSpecificCache()
        {
            var specificCacheImplementationMock = new Mock<ICacheImplementation>();
            var cacheOptions = new CacheOptions 
            { 
                CacheImplementation = specificCacheImplementationMock.Object 
            };
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options)
                .Returns(cacheOptions);
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromCachedObject(It.Is<User>(u => u.Name == "Test name")))
                .Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder)
                .Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>())
                .Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current)
                .Returns(genericCacheImplementationMock.Object);

            var user = new User { Name = "Test name" };
            await _sut.CacheAsync(user);

            genericCacheImplementationMock
                .Verify(c => c.CacheAsync(
                    It.IsAny<string>(), 
                    It.IsAny<User>(), 
                    It.IsAny<CacheOptions>()), 
                Times.Never);
            specificCacheImplementationMock
                .Verify(c => c.CacheAsync(
                    "User_Key", 
                    It.Is<User>(u => u.Name == "Test name"), 
                    It.IsAny<CacheOptions>()), 
                Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromCachedObject(It.Is<User>(u => u.Name == "Test name")), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task CacheAsync_OnlyGenericCacheImplementation_UsesGenericCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options)
                .Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromCachedObject(It.Is<User>(u => u.Name == "Test name")))
                .Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder)
                .Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>())
                .Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current)
                .Returns(genericCacheImplementationMock.Object);

            var user = new User { Name = "Test name" };
            await _sut.CacheAsync(user);

            genericCacheImplementationMock
                .Verify(c => c.CacheAsync(
                    "User_Key", 
                    It.Is<User>(u => u.Name == "Test name"), 
                    It.IsAny<CacheOptions>()), 
                Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromCachedObject(It.Is<User>(u => u.Name == "Test name")), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsyncComplexKey_MissingCachingPolicy_ThrowsException()
        {
            await _sut.Invoking(s => s.RetrieveAsync<User>(new { }))
                .Should()
                .ThrowAsync<ConfigurationNotFoundException>()
                .WithMessage("Caching configuration for type 'FluentCaching.Tests.Unit.Models.User' is not found");
        }

        [Fact]
        public async Task RetrieveAsyncComplexKey_MissingCacheImplementation_ThrowsException()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options)
                .Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            configurationItemMock
                .SetupGet(i => i.KeyBuilder)
                .Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>())
                .Returns(configurationItemMock.Object);

            await _sut.Invoking(s => s.RetrieveAsync<User>(new { }))
                .Should()
                .ThrowAsync<CacheImplementationNotFoundException>()
                .WithMessage("No caching implementation configured for type - 'FluentCaching.Tests.Unit.Models.User'");
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromObjectKey(new { }), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
            _cacheConfigurationMock
                .VerifyGet(c => c.Current, Times.AtLeastOnce);
        }

        [Fact]
        public async Task RetrieveAsyncComplexKey_SpecificCacheImplementation_UsesSpecificCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            var specificCacheImplementationMock = new Mock<ICacheImplementation>();
            var cacheOptions = new CacheOptions 
            { 
                CacheImplementation = specificCacheImplementationMock.Object 
            };
            configurationItemMock
                .SetupGet(i => i.Options)
                .Returns(cacheOptions);
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromObjectKey(new { }))
                .Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder)
                .Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock.Setup(c => c.GetItem<User>())
                .Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current)
                .Returns(genericCacheImplementationMock.Object);

            await _sut.RetrieveAsync<User>(new { });

            genericCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Never);
            specificCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromObjectKey(new { }), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsyncComplexKey_OnlyGenericCacheImplementation_UsesGenericCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromObjectKey(new { })).Returns("CachedObject");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RetrieveAsync<User>(new { });

            genericCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromObjectKey(new { }), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsyncStaticKey_MissingCachingPolicy_ThrowsException()
        {
            await _sut.Invoking(s => s.RetrieveAsync<User>()).Should()
                .ThrowAsync<ConfigurationNotFoundException>()
                .WithMessage("Caching configuration for type 'FluentCaching.Tests.Unit.Models.User' is not found");
        }

        [Fact]
        public async Task RetrieveAsyncStaticKey_MissingCacheImplementation_ThrowsException()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);

            await _sut.Invoking(s => s.RetrieveAsync<User>()).Should()
                .ThrowAsync<CacheImplementationNotFoundException>()
                .WithMessage("No caching implementation configured for type - 'FluentCaching.Tests.Unit.Models.User'");
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStaticKey(), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
            _cacheConfigurationMock
                .VerifyGet(c => c.Current, Times.AtLeastOnce);
        }

        [Fact]
        public async Task RetrieveAsyncStaticKey_SpecificCacheImplementation_UsesSpecificCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            var specificCacheImplementationMock = new Mock<ICacheImplementation>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions { CacheImplementation = specificCacheImplementationMock.Object });
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromStaticKey()).Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RetrieveAsync<User>();

            genericCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Never);
            specificCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStaticKey(), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsyncStaticKey_OnlyGenericCacheImplementation_UsesGenericCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromStaticKey()).Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RetrieveAsync<User>();

            genericCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStaticKey(), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsyncSimpleKey_MissingCachingPolicy_ThrowsException()
        {
            await _sut.Invoking(s => s.RetrieveAsync<User>("CachedObject")).Should()
                .ThrowAsync<ConfigurationNotFoundException>()
                .WithMessage("Caching configuration for type 'FluentCaching.Tests.Unit.Models.User' is not found");
        }

        [Fact]
        public async Task RetrieveAsyncSimpleKey_MissingCacheImplementation_ThrowsException()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);

            await _sut.Invoking(s => s.RetrieveAsync<User>("CachedObject")).Should()
                .ThrowAsync<CacheImplementationNotFoundException>()
                .WithMessage("No caching implementation configured for type - 'FluentCaching.Tests.Unit.Models.User'");
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStringKey("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
            _cacheConfigurationMock
                .VerifyGet(c => c.Current, Times.AtLeastOnce);
        }

        [Fact]
        public async Task RetrieveAsyncSimpleKey_SpecificCacheImplementation_UsesSpecificCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            var specificCacheImplementationMock = new Mock<ICacheImplementation>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions { CacheImplementation = specificCacheImplementationMock.Object });
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromStringKey("CachedObject")).Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RetrieveAsync<User>("CachedObject");

            genericCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Never);
            specificCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStringKey("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RetrieveAsyncSimpleKey_OnlyGenericCacheImplementation_UsesGenericCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromStringKey("CachedObject")).Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RetrieveAsync<User>("CachedObject");

            genericCacheImplementationMock
                .Verify(c => c.RetrieveAsync<User>("User_Key"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStringKey("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RemoveAsyncComplexKey_MissingCachingPolicy_ThrowsException()
        {
            await _sut.Invoking(s => s.RemoveAsync<User>(new { })).Should()
                .ThrowAsync<ConfigurationNotFoundException>()
                .WithMessage("Caching configuration for type 'FluentCaching.Tests.Unit.Models.User' is not found");
        }

        [Fact]
        public async Task RemoveAsyncComplexKey_MissingCacheImplementation_ThrowsException()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);

            await _sut.Invoking(s => s.RemoveAsync<User>(new { })).Should()
                .ThrowAsync<CacheImplementationNotFoundException>()
                .WithMessage("No caching implementation configured for type - 'FluentCaching.Tests.Unit.Models.User'");
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromObjectKey(new { }), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
            _cacheConfigurationMock
                .VerifyGet(c => c.Current, Times.AtLeastOnce);
        }

        [Fact]
        public async Task RemoveAsyncComplexKey_SpecificCacheImplementation_UsesSpecificCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            var specificCacheImplementationMock = new Mock<ICacheImplementation>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions { CacheImplementation = specificCacheImplementationMock.Object });
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromObjectKey(new { })).Returns("CachedObject");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RemoveAsync<User>(new { });

            genericCacheImplementationMock
                .Verify(c => c.RemoveAsync("CachedObject"), Times.Never);
            specificCacheImplementationMock
                .Verify(c => c.RemoveAsync("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromObjectKey(new { }), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_ComplexKeyWithOnlyGenericCacheImplementation_UsesGenericCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromObjectKey(new { })).Returns("CachedObject");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RemoveAsync<User>(new { });

            genericCacheImplementationMock
                .Verify(c => c.RemoveAsync("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromObjectKey(new { }), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_SimpleKeyWithMissingCachingPolicy_ThrowsException()
        {
            await _sut.Invoking(s => s.RemoveAsync<User>("CachedObject")).Should()
                .ThrowAsync<ConfigurationNotFoundException>()
                .WithMessage("Caching configuration for type 'FluentCaching.Tests.Unit.Models.User' is not found");
        }

        [Fact]
        public async Task RemoveAsync_SimpleKeyWithMissingCacheImplementation_ThrowsException()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);

            await _sut.Invoking(s => s.RemoveAsync<User>("CachedObject")).Should()
                .ThrowAsync<CacheImplementationNotFoundException>()
                .WithMessage("No caching implementation configured for type - 'FluentCaching.Tests.Unit.Models.User'");
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStringKey("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
            _cacheConfigurationMock
                .VerifyGet(c => c.Current, Times.AtLeastOnce);
        }

        [Fact]
        public async Task RemoveAsync_SimpleKeyWithSpecificCacheImplementation_UsesSpecificCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            var specificCacheImplementationMock = new Mock<ICacheImplementation>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions { CacheImplementation = specificCacheImplementationMock.Object });
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromStringKey("CachedObject")).Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RemoveAsync<User>("CachedObject");

            genericCacheImplementationMock
                .Verify(c => c.RemoveAsync("User_Key"), Times.Never);
            specificCacheImplementationMock
                .Verify(c => c.RemoveAsync("User_Key"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStringKey("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_SimpleKeyWithOnlyGenericCacheImplementation_UsesGenericCache()
        {
            var configurationItemMock = new Mock<ICacheConfigurationItem<User>>();
            configurationItemMock
                .SetupGet(i => i.Options).Returns(new CacheOptions());
            var propertyTrackerMock = new Mock<IKeyBuilder<User>>();
            propertyTrackerMock
                .Setup(p => p.BuildFromStringKey("CachedObject")).Returns("User_Key");
            configurationItemMock
                .SetupGet(i => i.KeyBuilder).Returns(propertyTrackerMock.Object);
            _cacheConfigurationMock
                .Setup(c => c.GetItem<User>()).Returns(configurationItemMock.Object);
            var genericCacheImplementationMock = new Mock<ICacheImplementation>();
            _cacheConfigurationMock
                .SetupGet(c => c.Current).Returns(genericCacheImplementationMock.Object);

            await _sut.RemoveAsync<User>("CachedObject");

            genericCacheImplementationMock
                .Verify(c => c.RemoveAsync("User_Key"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.Options, Times.AtLeastOnce);
            propertyTrackerMock
                .Verify(p => p.BuildFromStringKey("CachedObject"), Times.Once);
            configurationItemMock
                .VerifyGet(i => i.KeyBuilder, Times.AtLeastOnce);
            _cacheConfigurationMock
                .Verify(c => c.GetItem<User>(), Times.Once);
        }
    }
}
