using Hackathon.Common.Models.EventLog;
using Hackathon.Entities;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class EventLogMappings: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EventLogModel, EventLogEntity>()
            .IgnoreNullValues(true)
            .Map(x=>x.Id, s=>s.Id)
            .Map(x => x.Type, s => s.LogType)
            .Map(x => x.UserId, s => s.UserId)
            .Map(x => x.Description, s => s.Description)
            .Map(x => x.Timestamp, s => s.Timestamp);

        config.ForType<EventLogEntity, EventLogListItem>()
            .IgnoreNullValues(true)
            .Map(x=>x.Id, s=>s.Id)
            .Map(x => x.LogType, s => s.Type)
            .Map(x => x.UserId, s => s.UserId)
            .Map(x => x.UserName, s => s.UserName)
            .Map(x => x.Description, s => s.Description)
            .Map(x => x.Timestamp, s => s.Timestamp);
    }
}
