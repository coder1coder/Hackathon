using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class TeamEntityMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .ForType<TeamEntity, TeamModel>()
                .PreserveReference(true)
                .Map(x => x.Event, s => s.Event)
                .Map(x => x.Users, s => s.Users)
                .Map(x => x.Project, s=>s.Project)
                .MaxDepth(2);
                ;
        }
    }
}