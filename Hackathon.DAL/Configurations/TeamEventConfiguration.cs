using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class TeamEventConfiguration : IEntityTypeConfiguration<TeamEventEntity>
{
    public void Configure(EntityTypeBuilder<TeamEventEntity> builder)
    {
        builder.ToTable("TeamEvents");
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.TeamEvents)
            .HasForeignKey(x => x.EventId);

        builder
            .HasOne(x => x.Team)
            .WithMany(x => x.TeamEvents)
            .HasForeignKey(x => x.TeamId);

        builder
            .HasOne(x => x.Project)
            .WithOne(x => x.TeamEvent)
            .HasForeignKey<ProjectEntity>(x => x.TeamEventId);
    }
}