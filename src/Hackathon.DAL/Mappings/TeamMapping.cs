using System.Linq;
using Hackathon.DAL.Entities;
using Mapster;
using System.Collections.Generic;
using Hackathon.Common.Models.Teams;
using Hackathon.DAL.Entities.Teams;

namespace Hackathon.DAL.Mappings;

public class TeamMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<CreateTeamModel, TeamEntity>()
            .IgnoreNullValues(true);

        config.ForType<MemberTeamEntity, TeamMember>()
            .Map(d => d, d => d.Member)
            .Map(d => d.DateTimeAdd, src => src.DateTimeAdd)
            .Map(d => d.TeamRole, src => src.Role);

        config
            .ForType<TeamEntity, TeamModel>()
            .IgnoreNullValues(true)
            .MaxDepth(3)
            .AfterMapping((s, d) =>
            {
                var members = new List<TeamMember>(d.Members);
                if (d.Owner is not null && members.All(x => x.Id != d.OwnerId))
                {
                    members.Add(d.Owner);
                }

                d.Members = members.ToArray();
            });

        config.ForType<TeamJoinRequestEntity, TeamJoinRequestModel>()
            .Map(x=>x.TeamName, s=>s.Team.Name, z=> z.Team != null)
            .Map(x=>x.TeamOwnerId, s=>s.Team.OwnerId, z=>z.Team != null)
            .Map(x=>x.UserName, s=>s.User.GetAnyName(), z=>z.User != null);
    }
}
