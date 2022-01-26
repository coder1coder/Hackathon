using System.Linq;
using Hackathon.Common.Models.Event;
using Hackathon.DAL.Entities;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class EventEntityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<EventEntity, EventModel>()
                .IgnoreNullValues(true)
                .Map(x=>x.TeamEvents, s=>s.TeamEvents)
                //Attention! Max depth is 3 because in case when it equal 4 and more, we will have exception "circular json"
                .MaxDepth(3);

            //for fake in tests
            config
                .ForType<EventEntity, CreateEventModel>()
                .IgnoreNullValues(true);

        }
    }
}