using System.Collections.Generic;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.EventStage;
using Hackathon.Common.Models.Tags;
using Hackathon.DAL.Entities.Event;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class EventMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IEnumerable<EventStageModel>, ICollection<EventStageEntity>>()
            .MapCollections((s, d) => s.Id != default && s.Id == d.Id);

        config.ForType<EventCreateParameters, EventEntity>()
            .Inherits<IHasArrayTags, IHasStringTags>()
            .Map(x => x.Start, s => s.Start.ToUniversalTime());

        config.ForType<EventUpdateParameters, EventEntity>()
            .Inherits<IHasArrayTags, IHasStringTags>()
            .Map(x => x.Start, s => s.Start.ToUniversalTime())
            .Map(x => x.Agreement.EventId, s => s.Id, x => x.Agreement != null);

        config.ForType<EventEntity, EventModel>()
            .Inherits<IHasStringTags, IHasArrayTags>()
            .IgnoreNullValues(true)
            .MaxDepth(5);

        //for fake in tests
        config
            .ForType<EventEntity, EventCreateParameters>()
            .Inherits<IHasStringTags, IHasArrayTags>()
            .IgnoreNullValues(true)
            .MaxDepth(3);

        config.ForType<EventAgreementModel, EventAgreementEntity>()
            .Ignore(x=>x.Event);
    }
}
