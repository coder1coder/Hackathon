using BenchmarkDotNet.Attributes;
using Mapster;
using MapsterMapper;

namespace Hackathon.Tests.Benchmark.Mapping;

[MinColumn, MaxColumn]
public class MapsterCastBenchmark
{
    private readonly IMapper _mapper;

    [Params(30_000)] 
    public int ModelsCount { get; set; }

    public MapsterCastBenchmark()
    {
        var config = new TypeAdapterConfig();
        config.Apply(new MappingProfile());

        _mapper = new Mapper(config);
    }

    [Benchmark]
    public void MapWithoutDestination()
    {
        var list = GetListForTest();
        _mapper.Map<List<ModelB>>(list);
    }
    
    [Benchmark]
    public void MapWithDestination()
    {
        var list = GetListForTest();
        _mapper.Map<List<ModelA>, List<ModelB>>(list);
    }

    private List<ModelA> GetListForTest()
    {
        var fromModels = new List<ModelA>();
        for (var i = 0; i < ModelsCount; i++)
            fromModels.Add(new ModelA
            {
                Id = i, 
                Name = Guid.NewGuid().ToString()
            });

        return fromModels;
    }
}

public class MappingProfile: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<ModelA, ModelB>()
            .Map(x => x.Id, s => s.Id)
            .Map(x => x.Name, s => s.Name);
    }
}

public class ModelA
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

public class ModelB
{
    public long Id { get; set; }
    public string? Name { get; set; }
}