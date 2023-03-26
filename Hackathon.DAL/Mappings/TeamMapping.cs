using System.Linq;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Hackathon.DAL.Entities;
using Mapster;
using System.Collections.Generic;

namespace Hackathon.DAL.Mappings;

public class TeamMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<CreateTeamModel, TeamEntity>()
            .IgnoreNullValues(true);

        config
            .ForType<TeamEntity, TeamModel>()
            .IgnoreNullValues(true)
            .Map(x => x.Members, s =>
                s.Members.Select(x => x.Member))
            .MaxDepth(3)
            .AfterMapping((_, d) =>
            {
                var members = new List<UserModel>(d.Members);
                if (d.Owner is not null && members.All(x => x.Id != d.OwnerId))
                    members.Add(d.Owner);

                d.Members = members.ToArray();
            });

        config.ForType<TeamJoinRequestEntity, TeamJoinRequestModel>()
            .Map(x=>x.TeamName, s=>s.Team.Name, z=> z.Team != null)
            .Map(x=>x.TeamOwnerId, s=>s.Team.OwnerId, z=>z.Team != null)
            .Map(x=>x.UserName, s=>s.User.GetAnyName(), z=>z.User != null);
    }
}
