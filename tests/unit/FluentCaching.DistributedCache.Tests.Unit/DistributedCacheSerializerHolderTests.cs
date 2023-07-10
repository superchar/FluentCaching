using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using static FluentCaching.DistributedCache.Tests.Unit.MockHelper;

namespace FluentCaching.DistributedCache.Tests.Unit;

public class DistributedCacheSerializerHolderTests
{
    private readonly Mock<IDistributedCacheSerializer> _firstSerializerMock;
    private readonly Mock<IDistributedCacheSerializer> _secondSerializerMock;
    private Mock<IServiceScope> _serviceScopeMock;
    private Mock<IServiceProvider> _serviceScopeProviderMock;
    
    public DistributedCacheSerializerHolderTests()
    {
        _firstSerializerMock = new Mock<IDistributedCacheSerializer>();
        _secondSerializerMock = new Mock<IDistributedCacheSerializer>();
        MockScopeAndServiceProvider();
    }

    [Fact]
    public void Serializer_SerializerParameterIsProvided_UsesProvidedSerializers()
    {
        _firstSerializerMock
            .Setup(s => s.CanBeUsedForType(typeof(User)))
            .Returns(true);
        var holder =
            new DistributedCacheSerializerHolder<User>(
                new[] {_firstSerializerMock.Object, _secondSerializerMock.Object});

        var resultSerializer = holder.Serializer;

        resultSerializer.Should().Be(_firstSerializerMock.Object);
    }
    
    [Fact]
    public void Serializer_SerializerParameterIsNotProvided_UsesServiceProviderToGetSerializers()
    {
        _firstSerializerMock
            .Setup(s => s.CanBeUsedForType(typeof(User)))
            .Returns(true);
        MockServiceProvider();
        var holder = new DistributedCacheSerializerHolder<User>();

        var resultSerializer = holder.Serializer;

        resultSerializer.Should().Be(_firstSerializerMock.Object);
    }

    [Fact]
    public void Serializer_CustomSerializerIsNotFound_UsesDefaultJsonSerializer()
    {
        var holder =
            new DistributedCacheSerializerHolder<User>(
                new[] {_firstSerializerMock.Object, _secondSerializerMock.Object});

        var resultSerializer = holder.Serializer;

        resultSerializer.Should().BeOfType<JsonDistributedCacheSerializer>();
    }
    
    [Fact]
    public void Serializer_MultipleSerializersMatch_UsesFirstMatchedSerializer()
    {
        var thirdSerializerMock = new Mock<IDistributedCacheSerializer>();
        thirdSerializerMock
            .Setup(s => s.CanBeUsedForType(typeof(User)))
            .Returns(true);
        _secondSerializerMock
            .Setup(s => s.CanBeUsedForType(typeof(User)))
            .Returns(true);
        var holder =
            new DistributedCacheSerializerHolder<User>(
                new[] {_firstSerializerMock.Object, _secondSerializerMock.Object, thirdSerializerMock.Object});

        var resultSerializer = holder.Serializer;

        resultSerializer.Should().Be(_secondSerializerMock.Object);
    }
    
    [Fact]
    public void Dispose_SerializerParameterIsNotProvided_DisposesCreatedScope()
    {
        MockServiceProvider();
        var holder =
            new DistributedCacheSerializerHolder<User>();
        
        holder.Dispose();
        
        _serviceScopeMock.Verify(s => s.Dispose(), Times.Once);
    }
    
    [Fact]
    public void Dispose_SerializerParameterIsProvided_DoesNotCreateAndDisposeScope()
    {
        var holder =
            new DistributedCacheSerializerHolder<User>(
                new[] {_firstSerializerMock.Object, _secondSerializerMock.Object});
        
        holder.Dispose();
        
        _serviceScopeMock.Verify(s => s.Dispose(), Times.Never);
    }

    private void MockServiceProvider()
        => _serviceScopeProviderMock
            .Setup(s => s.GetService(typeof(IEnumerable<IDistributedCacheSerializer>)))
            .Returns(new[] {_firstSerializerMock.Object, _secondSerializerMock.Object});

    private void MockScopeAndServiceProvider()
    {
        var (serviceScopeMock, serviceScopeProviderMock) = MockServiceLocator();
        _serviceScopeMock = serviceScopeMock;
        _serviceScopeProviderMock = serviceScopeProviderMock;
    }
}