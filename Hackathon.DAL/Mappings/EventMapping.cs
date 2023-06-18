using System.Collections.Generic;
using System.Linq;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.EventStage;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Entities.User;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class EventMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IEnumerable<EventStageModel>, ICollection<EventStageEntity>>()
            .MapCollections((s, d) => s.Id != default && s.Id == d.Id);

        config.ForType<EventCreateParameters, EventEntity>()
            .Map(x => x.Start, s => s.Start.ToUniversalTime());

        config.ForType<EventUpdateParameters, EventEntity>()
            .Map(x => x.Start, s => s.Start.ToUniversalTime())
            .Map(x => x.Agreement.EventId, s => s.Id, x => x.Agreement != null);

        config.ForType<EventEntity, EventModel>()
            .IgnoreNullValues(true)
            .MaxDepth(5);

        //for fake in tests
        config
            .ForType<EventEntity, EventCreateParameters>()
            .IgnoreNullValues(true)
            .MaxDepth(3);

        config.ForType<EventAgreementModel, EventAgreementEntity>()
            .Ignore(x=>x.Event);
    }
}
