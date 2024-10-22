using System.Linq;
using AutoFixture;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Tags;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Mappings;
using Mapster;
using MapsterMapper;
using Xunit;

namespace Hackathon.DAL.Tests;

public class MappingTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public MappingTests()
    {
        var config = new TypeAdapterConfig();
        config.Scan(typeof(EventMapping).Assembly);

        _mapper = new Mapper(config);
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _fixture.Register(() =>
            new TagCollection(_fixture.CreateMany<string>().ToArray()));
    }

    [Fact]
    public void EventEntity_EventModel_Tags()
    {
        //arrange
        var source = _fixture
            .Build<EventEntity>()
            .Create();
        
        //act
        var destination = _mapper.Map<EventEntity, EventModel>(source);
        
        //assert
        var sourceTagsAsArray = source.Tags.Split(TagsMappings.TagsSeparator);
        destination.Tags.Should().BeEquivalentTo(sourceTagsAsArray);
    }
    
    [Fact]
    public void EventCreateParameters_EventEntity_Tags()
    {
        //arrange
        var source = _fixture
            .Build<EventCreateParameters>()
            .Create();
        
        //act
        var destination = _mapper.Map<EventCreateParameters, EventEntity>(source);
        
        //assert
        var sourceTagsAsString = string.Join(TagsMappings.TagsSeparator, source.Tags);
        destination.Tags.Should().BeEquivalentTo(sourceTagsAsString);
    }
}
