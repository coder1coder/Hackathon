using Hackathon.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class ProjectMemberEntityConfiguration: IEntityTypeConfiguration<ProjectMemberEntity>
{
    public void Configure(EntityTypeBuilder<ProjectMemberEntity> builder)
    {
        builder.ToTable("ProjectMembers");
        builder.HasKey(x => x.Id);
        builder
            .HasOne(x => x.User);
        builder
            .HasOne(x => x.Project);
        builder
            .HasOne(x => x.Team);
        builder
            .HasOne(x => x.Event);
    }
}