using Hackathon.Common.Models.Block;
using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities;
using Hackathon.DAL.Entities.Block;
using Mapster;
using System.Collections.Generic;

namespace Hackathon.DAL.Mappings;

public class BlockMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<BlockingEntity, BlockingModel>();

        config
            .ForType<BlockingModel, BlockingEntity>();
    }
}
