using Hackathon.DAL.Mappings;
using Mapster;
using MapsterMapper;
using Xunit;

namespace Hackathon.BL.Tests;

[Trait("Category","Unit")]
public abstract class BaseUnitTest
{
    protected readonly Mapper Mapper;

    protected BaseUnitTest()
    {
        var config = new TypeAdapterConfig();
        config.Scan(typeof(EventMapping).Assembly);

        Mapper = new Mapper(config);
    }
}
