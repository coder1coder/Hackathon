using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Hackathon.Entities;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class EventEntityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<EventCreateParameters, EventEntity>()
                .IgnoreNullValues(true)
                .Map(x => x.Start, s => s.Start.ToUniversalTime());

            config.ForType<EventUpdateParameters, EventEntity>()
                .IgnoreNullValues(true)
                .Map(x => x.Start, s => s.Start.ToUniversalTime());

            config.ForType<EventEntity, EventModel>()
                .Map(x=>x.Stages, s=>new List<EventStage>
                {
                    new()
                    {
                        Status = EventStatus.Published,
                        Duration = s.MemberRegistrationMinutes
                    },
                    new()
                    {
                        Status = EventStatus.Development,
                        Duration = s.DevelopmentMinutes
                    },
                    new()
                    {
                        Status = EventStatus.Presentation,
                        Duration = s.TeamPresentationMinutes
                    }
                })
                .IgnoreNullValues(true)
                .MaxDepth(5);

            //for fake in tests
            config
                .ForType<EventEntity, EventCreateParameters>()
                .IgnoreNullValues(true)
                .MaxDepth(3);

        }
    }
}
