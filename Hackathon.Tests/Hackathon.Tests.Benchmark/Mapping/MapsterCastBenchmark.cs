using BenchmarkDotNet.Attributes;
using IMapper = MapsterMapper.IMapper;

namespace Hackathon.Tests.Benchmark.Mapping;

[MinColumn, MaxColumn]
public class MapsterCastBenchmark
{
    private readonly IMapper _mapper;

    [Params(30_000)] 
    public int ModelsCount { get; set; }

    public MapsterCastBenchmark()
    {
        TestHelper.InitMapster(out _mapper);
    }

    [Benchmark]
    public void MapWithoutDestination()
    {
        _mapper.Map<List<ModelB>>(TestHelper.GetListForTest(ModelsCount));
    }
    
    [Benchmark]
    public void MapWithDestination()
    {
        _mapper.Map<List<ModelA>, List<ModelB>>(TestHelper.GetListForTest(ModelsCount));
    }
}