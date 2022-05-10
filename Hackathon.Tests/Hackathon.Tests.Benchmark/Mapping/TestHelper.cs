using AutoMapper;
using Mapster;

namespace Hackathon.Tests.Benchmark.Mapping;

public static class TestHelper
{
    public static void InitMapster(out MapsterMapper.IMapper mapster)
    {
        var mapsterConfig = new TypeAdapterConfig();
        mapsterConfig.Apply(new MappingProfile());
        mapster = new MapsterMapper.Mapper(mapsterConfig);
    }

    public static void InitAutomapper(out AutoMapper.IMapper automapper)
    {
        var automapperConfig = new MapperConfiguration(x => 
            x.AddProfile(new MappingProfile()));
        automapper = new AutoMapper.Mapper(automapperConfig);
    }
    
    public static List<ModelA> GetListForTest(int modelsCount)
    {
        var fromModels = new List<ModelA>();
        for (var i = 0; i < modelsCount; i++)
            fromModels.Add(new ModelA
            {
                Id = i, 
                Name = Guid.NewGuid().ToString()
            });

        return fromModels;
    }
}