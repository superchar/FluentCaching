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
        yield return ["test"];
        yield return [Guid.NewGuid()];
        yield return [27.5m];
        yield return [true];
        yield return [(byte)42];
        yield return [(sbyte)43];
        yield return [(short)44];
        yield return [(ushort)45];
        yield return [46];
        yield return [47U];
        yield return [48L];
        yield return [48UL];
        yield return ['a'];
        yield return [42.42d];
        yield return [42.43f];
    }
}