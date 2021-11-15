using Hackathon.Common.Entities;
using Hackathon.Common.Models.Team;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class TeamEntityMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .ForType<TeamEntity, TeamModel>()

                .IgnoreNonMapped(true)
                .PreserveReference(true)

                .Map(x => x.Event, s => s.Event)
                .Map(x => x.Users, s => s.Users);
        }
    }
}