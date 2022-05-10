using BenchmarkDotNet.Attributes;
using IMapper = MapsterMapper.IMapper;

namespace Hackathon.Tests.Benchmark.Mapping;

[MinColumn, MaxColumn]
public class MapsterVsAutomapper
{
    private readonly IMapper _mapster;
    private readonly AutoMapper.IMapper _automapper;
    
    [Params(50_000)]
    public int ModelsCount { get; set; }

    [Benchmark]
    public void MapsterMap()
    {
        _mapster.Map<List<ModelA>, List<ModelB>>(TestHelper.GetListForTest(ModelsCount));    
    }
    
    [Benchmark]
    public void AutomapperMap()
    {
        _automapper.Map<List<ModelA>, List<ModelB>>(TestHelper.GetListForTest(ModelsCount));    
    }
    
    public MapsterVsAutomapper()
    {
        TestHelper.InitMapster(out _mapster);
        TestHelper.InitAutomapper(out _automapper);
    }
}