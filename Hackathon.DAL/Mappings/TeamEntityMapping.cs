using System.Collections.Generic;
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
                .ForType<CreateTeamModel, TeamEntity>()
                .PreserveReference(true)
                .IgnoreNullValues(true)
                .Map(x => x.Name, s => s.Name)
                .Map(x => x.OwnerId, s => s.OwnerId)
                .Map(x => x.TeamEvents, s =>  new List<TeamEventEntity>()
                {
                    new TeamEventEntity()
                    {
                        EventId = s.EventId
                    }
                }, c =>c.EventId > 0);

            config
                .ForType<TeamEventEntity, TeamEventModel>()
                .PreserveReference(true)
                .IgnoreNullValues(true)
                .Map(x => x.Event, s => s.Event)
                .Map(x => x.Team, s => s.Team)
                .Map(x => x.Project, s => s.Project)
                .MaxDepth(2);

            config
                .ForType<TeamEntity, TeamModel>()
                .PreserveReference(true)
                .IgnoreNullValues(true)
                .Map(x => x.TeamEvents, s => s.TeamEvents)
                .Map(x => x.Users, s => s.Users)
                .Map(x => x.Owner, x => x.Owner);
        }
    }
}