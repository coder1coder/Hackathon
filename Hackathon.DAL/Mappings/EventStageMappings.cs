using Hackathon.Common.Models.EventStage;
using Hackathon.Entities.Event;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class EventStageMappings: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EventStageModel, EventStageEntity>()
            .IgnoreNullValues(true);
    }
}
