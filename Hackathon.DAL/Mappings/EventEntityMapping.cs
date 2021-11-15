using Hackathon.Common.Entities;
using Hackathon.Common.Models.Event;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class EventEntityMapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .ForType<EventEntity, EventModel>()

                .PreserveReference(true)

                .Map(x => x.Teams, s => s.Teams);
        }
    }
}