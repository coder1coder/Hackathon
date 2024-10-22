using Hackathon.DAL.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.Projects;

public class ProjectEntityConfiguration: IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("Projects");

        builder
            .Property(x => x.Name)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasKey(x => new {x.EventId, x.TeamId});

        builder.HasOne(x => x.Event);
        builder.HasOne(x => x.Team);
    }
}
