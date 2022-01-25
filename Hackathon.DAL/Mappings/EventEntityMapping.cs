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
                .MaxDepth(3);
        }
    }
}