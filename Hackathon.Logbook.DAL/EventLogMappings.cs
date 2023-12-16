using Hackathon.Logbook.Abstraction.Models;
using Hackathon.Logbook.DAL.Entities;
using Mapster;

namespace Hackathon.Logbook.DAL;

public class EventLogMappings: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EventLogModel, EventLogEntity>()
            .IgnoreNullValues(true)
            .Map(x => x.Type, s => s.LogType);

        config.ForType<EventLogEntity, EventLogListItem>()
            .IgnoreNullValues(true)
            .Map(x => x.LogType, s => s.Type);
    }
}
