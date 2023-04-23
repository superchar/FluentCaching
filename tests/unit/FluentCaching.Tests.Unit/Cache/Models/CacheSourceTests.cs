using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentCaching.Cache.Models;
using FluentCaching.Tests.Unit.TestModels;
using Xunit;

namespace FluentCaching.Tests.Unit.Cache.Models;

public class CacheSourceTests
{
    [Fact]
    public void Create_KeyIsNull_CreatesStaticCacheSource()
    {
        var cacheSource = CacheSource<User>.Create(null);

        cacheSource.CacheSourceType.Should().Be(CacheSourceType.Static);
    }
    
    [Fact]
    public void Create_KeyIsObject_CreatesComplexCacheSource()
    {
        var key = new object();
        var cacheSource = CacheSource<User>.Create(key);

        cacheSource.Key.Should().Be(key);
        cacheSource.CacheSourceType.Should().Be(CacheSourceType.Complex);
    }
    
    [Theory]
    [MemberData(nameof(GetScalarKeys))]
    public void Create_KeyIsScalar_CreatesScalarCacheSource(object key)
    {
        var cacheSource = CacheSource<User>.Create(key);

        cacheSource.Key.Should().Be(key);
        cacheSource.CacheSourceType.Should().Be(CacheSourceType.Scalar);
    }

    public static IEnumerable<object[]> GetScalarKeys()
    {
        yield return new object[] { "test" };
        yield return new object[] { Guid.NewGuid() };
        yield return new object[] { 27.5m };
        yield return new object[] { true };
        yield return new object[] { (byte)42 };
        yield return new object[] { (sbyte)43 };
        yield return new object[] { (short)44 };
        yield return new object[] { (ushort)45 };
        yield return new object[] { 46 };
        yield return new object[] { 47U };
        yield return new object[] { 48L };
        yield return new object[] { 48UL };
        yield return new object[] { 'a' };
        yield return new object[] { 42.42d };
        yield return new object[] { 42.43f };
    }
}