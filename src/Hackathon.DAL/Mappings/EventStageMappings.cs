using Hackathon.Common.Models.EventStage;
using Hackathon.DAL.Entities.Event;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class EventStageMappings: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EventStageModel, EventStageEntity>()
            .IgnoreNullValues(true);

        config.ForType<EventStageEntity, EventStageModel>()
            .Map(x=>x.Id, s=>s.Id)
            .IgnoreNullValues(true);
    }
}
