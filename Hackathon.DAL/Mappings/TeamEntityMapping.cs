using Hackathon.Common.Models.Team;
using Hackathon.Entities;
using Mapster;
using System.Linq;

namespace Hackathon.DAL.Mappings
{
    public class TeamEntityMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .ForType<CreateTeamModel, TeamEntity>()
                .IgnoreNullValues(true);

            config
                .ForType<TeamEntity, TeamModel>()
                .IgnoreNullValues(true)
                .Map(x => x.Members, s => s.Members.Select(x => x.Member))
                .MaxDepth(3);
        }
    }
}

