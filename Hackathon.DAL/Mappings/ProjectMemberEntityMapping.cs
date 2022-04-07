using System.Collections.Generic;
using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models.ProjectMember;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class ProjectMemberEntityMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<CreateProjectMemberModel, ProjectMemberEntity>()
            .IgnoreNullValues(true)
            .Map(x => x.UserId, s => s.UserId)
            .Map(x => x.TeamId, s => s.TeamId)
            .Map(x => x.ProjectId, s => s.ProjectId)
            .Map(x => x.ProjectMemberRoles,
                s => new List<string>());

        config
            .ForType<CreateProjectMemberModel, ProjectMemberModel>()
            .IgnoreNullValues(true)
            .Map(x => x.UserId, s => s.UserId)
            .Map(x => x.TeamId, s => s.TeamId)
            .Map(x => x.ProjectId, s => s.ProjectId)
            .Map(x => x.ProjectMemberRoles,
                s => new List<string>());

        config
            .ForType<ProjectMemberEntity, ProjectMemberModel>()
            .IgnoreNullValues(true)
            .Map(x => x.UserId, s => s.UserId)
            .Map(x => x.TeamId, s => s.TeamId)
            .Map(x => x.ProjectId, s => s.ProjectId)
            .MaxDepth(3);
    }
}