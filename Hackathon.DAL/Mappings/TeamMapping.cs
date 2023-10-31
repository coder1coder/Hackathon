using System.Linq;
using Hackathon.Common.Models.Team;
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
            .AfterMapping((s, d) =>
            {
                var members = new List<TeamMember>(d.Members);
                if (d.Owner is not null && members.All(x => x.Id != d.OwnerId))
                    members.Add(d.Owner);

                d.Members = members.ToArray();

                foreach (var teamMember in d.Members)
                {
                    var memberEntity = s.Members.FirstOrDefault(x => x.MemberId == teamMember.Id);

                    teamMember.DateTimeAdd = memberEntity?.DateTimeAdd ?? default;
                    teamMember.TeamRole = memberEntity?.Role ?? TeamRole.Participant;
                }
            });


        config.ForType<TeamJoinRequestEntity, TeamJoinRequestModel>()
            .Map(x=>x.TeamName, s=>s.Team.Name, z=> z.Team != null)
            .Map(x=>x.TeamOwnerId, s=>s.Team.OwnerId, z=>z.Team != null)
            .Map(x=>x.UserName, s=>s.User.GetAnyName(), z=>z.User != null);
    }
}
