using System;
using System.Threading.Tasks;
using FluentCaching.Cache;
using FluentCaching.Cache.Facades;
using FluentCaching.Tests.Unit.Models;
using Moq;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache;

public class CacheTests
{
    private static readonly User User = new();
        
    private readonly Mock<ICacheFacade> _cacheFacadeMock;

    private readonly ICache _cache;

    public CacheTests()
    {
        _cacheFacadeMock = new Mock<ICacheFacade>();

        _cache = new FluentCaching.Cache.Cache(_cacheFacadeMock.Object);
    }

    [Fact]
    public async Task CacheAsync_WhenCalled_InvokesCacheFacade()
    {
        await _cache.CacheAsync(User);

        _cacheFacadeMock.Verify(f => f.CacheAsync(User), Times.Once);
    }
        
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task RemoveAsync_BoolKey_InvokesCacheFacade(bool key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(byte.MaxValue)]
    [InlineData(byte.MinValue)]
    public async Task RemoveAsync_ByteKey_InvokesCacheFacade(byte key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData('a')]
    [InlineData('\n')]
    [InlineData('4')]
    [InlineData('B')]
    public async Task RemoveAsync_CharKey_InvokesCacheFacade(char key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(-1.43535)]
    [InlineData(555555555555555555)]
    [InlineData(-3423423523523342342)]
    public async Task RemoveAsync_DecimalKey_InvokesCacheFacade(decimal key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(-1.43535)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    public async Task RemoveAsync_DoubleKey_InvokesCacheFacade(double key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(-1.43535)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    public async Task RemoveAsync_FloatKey_InvokesCacheFacade(float key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public async Task RemoveAsync_IntKey_InvokesCacheFacade(int key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public async Task RemoveAsync_LongKey_InvokesCacheFacade(long key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(sbyte.MaxValue)]
    [InlineData(sbyte.MinValue)]
    public async Task RemoveAsync_SByteKey_InvokesCacheFacade(sbyte key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(short.MaxValue)]
    [InlineData(short.MinValue)]
    public async Task RemoveAsync_ShortKey_InvokesCacheFacade(short key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("test")]
    [InlineData("5353535")]
    [InlineData("TEST")]
    public async Task RemoveAsync_StringKey_InvokesCacheFacade(string key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(uint.MaxValue)]
    [InlineData(uint.MinValue)]
    public async Task RemoveAsync_UIntKey_InvokesCacheFacade(uint key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(ulong.MaxValue)]
    [InlineData(ulong.MinValue)]
    public async Task RemoveAsync_ULongKey_InvokesCacheFacade(ulong key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(ushort.MaxValue)]
    [InlineData(ushort.MinValue)]
    public async Task RemoveAsync_UShortKey_InvokesCacheFacade(ushort key)
    {
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData("b7c6ba42-aab5-4b22-b13b-3d072fa9971d")]
    [InlineData("5ba8dbaf-5786-4912-b02b-28ba07ee0b57")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task RemoveAsync_GuidKey_InvokesCacheFacade(string guidString)
    {
        var key = Guid.Parse(guidString);
            
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveScalarAsync<User>(key), Times.Once);
    }
        
    [Fact]
    public async Task RemoveAsync_ObjectKey_InvokesCacheFacade()
    {
        var key = new { };
            
        await _cache.RemoveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RemoveComplexAsync<User>(key), Times.Once);
    }
        
    [Fact]
    public async Task RemoveAsync_StaticKey_InvokesCacheFacade()
    {
        await _cache.RemoveAsync<User>();

        _cacheFacadeMock.Verify(f => f.RemoveStaticAsync<User>(), Times.Once);
    }
        
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task RetrieveAsync_BoolKey_InvokesCacheFacade(bool key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(byte.MaxValue)]
    [InlineData(byte.MinValue)]
    public async Task RetrieveAsync_ByteKey_InvokesCacheFacade(byte key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData('a')]
    [InlineData('\n')]
    [InlineData('4')]
    [InlineData('B')]
    public async Task RetrieveAsync_CharKey_InvokesCacheFacade(char key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(-1.43535)]
    [InlineData(555555555555555555)]
    [InlineData(-3423423523523342342)]
    public async Task RetrieveAsync_DecimalKey_InvokesCacheFacade(decimal key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(-1.43535)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    public async Task RetrieveAsync_DoubleKey_InvokesCacheFacade(double key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(0.5)]
    [InlineData(-1.43535)]
    [InlineData(float.MaxValue)]
    [InlineData(float.MinValue)]
    public async Task RetrieveAsync_FloatKey_InvokesCacheFacade(float key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public async Task RetrieveAsync_IntKey_InvokesCacheFacade(int key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public async Task RetrieveAsync_LongKey_InvokesCacheFacade(long key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(sbyte.MaxValue)]
    [InlineData(sbyte.MinValue)]
    public async Task RetrieveAsync_SByteKey_InvokesCacheFacade(sbyte key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(short.MaxValue)]
    [InlineData(short.MinValue)]
    public async Task RetrieveAsync_ShortKey_InvokesCacheFacade(short key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("test")]
    [InlineData("5353535")]
    [InlineData("TEST")]
    public async Task RetrieveAsync_StringKey_InvokesCacheFacade(string key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(uint.MaxValue)]
    [InlineData(uint.MinValue)]
    public async Task RetrieveAsync_UIntKey_InvokesCacheFacade(uint key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(ulong.MaxValue)]
    [InlineData(ulong.MinValue)]
    public async Task RetrieveAsync_ULongKey_InvokesCacheFacade(ulong key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData(42)]
    [InlineData(0)]
    [InlineData(ushort.MaxValue)]
    [InlineData(ushort.MinValue)]
    public async Task RetrieveAsync_UShortKey_InvokesCacheFacade(ushort key)
    {
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Theory]
    [InlineData("b7c6ba42-aab5-4b22-b13b-3d072fa9971d")]
    [InlineData("5ba8dbaf-5786-4912-b02b-28ba07ee0b57")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public async Task RetrieveAsync_GuidKey_InvokesCacheFacade(string guidString)
    {
        var key = Guid.Parse(guidString);
            
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveScalarAsync<User>(key), Times.Once);
    }
        
    [Fact]
    public async Task RetrieveAsync_ObjectKey_InvokesCacheFacade()
    {
        var key = new { };
            
        await _cache.RetrieveAsync<User>(key);

        _cacheFacadeMock.Verify(f => f.RetrieveComplexAsync<User>(key), Times.Once);
    }
        
    [Fact]
    public async Task RetrieveAsync_StaticKey_InvokesCacheFacade()
    {
        await _cache.RetrieveAsync<User>();

        _cacheFacadeMock.Verify(f => f.RetrieveStaticAsync<User>(), Times.Once);
    }
}