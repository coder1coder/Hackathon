using Hackathon.Common.Models.Event;
using Hackathon.DAL.Entities;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class EventEntityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<EventEntity, EventModel>.NewConfig()
                .IgnoreNullValues(true)
                .MaxDepth(2)
                ;
        }
    }
}