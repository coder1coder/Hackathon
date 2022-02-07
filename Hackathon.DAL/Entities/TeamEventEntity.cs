using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Entities;

public class TeamEventEntity: BaseEntity, IEntityTypeConfiguration<TeamEventEntity>
{
    public long TeamId { get; set; }
    public TeamEntity Team { get; set; }

    public long EventId { get; set; }
    public EventEntity Event { get; set; }

    public long? ProjectId { get; set; }
    public ProjectEntity Project { get; set; }

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