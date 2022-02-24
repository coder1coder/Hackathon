using System.Collections.Generic;
using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models.Event;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class EventEntityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<CreateEventModel, EventEntity>()
                .IgnoreNullValues(true)
                .Map(x => x.Start, s => s.Start.ToUniversalTime());
            
            config.ForType<UpdateEventModel, EventEntity>()
                .IgnoreNullValues(true)
                .Map(x => x.Start, s => s.Start.ToUniversalTime());
                
            config.ForType<EventEntity, EventModel>()
                .IgnoreNullValues(true)
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
                .MaxDepth(5);

            //for fake in tests
            config
                .ForType<EventEntity, CreateEventModel>()
                .IgnoreNullValues(true)
                .MaxDepth(3);

        }
    }
}