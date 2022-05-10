using AutoMapper;
using Mapster;

namespace Hackathon.Tests.Benchmark.Mapping;

public class MappingProfile: Profile, IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<ModelA, ModelB>()
            .Map(x => x.Id, s => s.Id)
            .Map(x => x.Name, s => s.Name);
    }

    public MappingProfile()
    {
        CreateMap<ModelA, ModelB>();
    }
}