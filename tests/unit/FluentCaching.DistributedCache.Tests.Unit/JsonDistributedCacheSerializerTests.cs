using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace FluentCaching.DistributedCache.Tests.Unit;

public class JsonDistributedCacheSerializerTests
{
    private static readonly User User = new("User Name");

    private readonly JsonDistributedCacheSerializer _jsonDistributedCacheSerializer = new();

    [Fact]
    public async Task SerializeAsync_ObjectProvided_SerializesToByArray()
    {
        var userBytes = JsonSerializer.SerializeToUtf8Bytes(User);

        var resultBytes = await _jsonDistributedCacheSerializer.SerializeAsync(User);

        resultBytes.Should().BeEquivalentTo(userBytes);
    }

    [Fact]
    public async Task DeserializerAsync_ByteArrayProvided_DeserializesToObject()
    {
        var userBytes = JsonSerializer.SerializeToUtf8Bytes(User);

        var resultObject = await _jsonDistributedCacheSerializer.DeserializeAsync<User>(userBytes);

        resultObject.Should().BeEquivalentTo(User);
    }

    [Theory]
    [InlineData(typeof(User))]
    [InlineData(typeof(object))]
    [InlineData(typeof(string))]
    public void CanBeUsedForType_AnyTypeProvided_ReturnsTrue(Type type)
    {
        var result = _jsonDistributedCacheSerializer.CanBeUsedForType(type);

        result.Should().BeTrue();
    }
}