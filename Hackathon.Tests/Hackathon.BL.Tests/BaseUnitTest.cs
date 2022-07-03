using Hackathon.DAL.Mappings;
using Mapster;
using MapsterMapper;

namespace Hackathon.BL.Tests;

public abstract class BaseUnitTest
{
    protected readonly Mapper Mapper;

    protected BaseUnitTest()
    {
        var config = new TypeAdapterConfig();
        config.Scan(typeof(EventEntityMapping).Assembly, typeof(NotificationMapping).Assembly);

        Mapper = new Mapper(config);
    }
}
