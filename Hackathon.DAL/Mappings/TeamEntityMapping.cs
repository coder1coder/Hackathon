using System.Collections.Generic;
using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models.Team;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class TeamEntityMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .ForType<CreateTeamModel, TeamEntity>()
                .IgnoreNullValues(true)
                .Map(x => x.Name, s => s.Name)
                .Map(x => x.OwnerId, s => s.OwnerId)
                .Map(x => x.TeamEvents, s =>  new List<TeamEventEntity>()
                {
                    new TeamEventEntity()
                    {
                        EventId = s.EventId.Value
                    }
                }, c =>c.EventId.HasValue && c.EventId > 0);

            config
                .ForType<CreateTeamModel, TeamModel>()
                .IgnoreNullValues(true)
                .Map(x => x.Name, s => s.Name)
                .Map(x => x.OwnerId, s => s.OwnerId)
                .Map(x => x.TeamEvents, s =>  new List<TeamEventEntity>()
                {
                    new()
                    {
                        EventId = s.EventId.Value
                    }
                }, c =>c.EventId.HasValue && c.EventId > 0);

            config
                .ForType<TeamEventEntity, TeamEventModel>()
                .IgnoreNullValues(true)
                .Map(x => x.Event, s => s.Event)
                .Map(x => x.Team, s => s.Team)
                .Map(x => x.Project, s => s.Project)
                .MaxDepth(3);

            config
                .ForType<TeamEntity, TeamModel>()
                .IgnoreNullValues(true)
                .Map(x => x.TeamEvents, s => s.TeamEvents)
                .Map(x => x.Users, s => s.Users)
                .Map(x => x.Owner, x => x.Owner)
                .MaxDepth(3);
        }
    }
}