using Hackathon.DAL.Entities;
using Hackathon.DAL.Entities.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Hackathon.DAL.Entities.Teams;

namespace Hackathon.DAL.Configurations.Team;

public class TeamEntityConfiguration: IEntityTypeConfiguration<TeamEntity>
{
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        builder.ToTable("Teams");
        builder.HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();

        builder
            .Property(x => x.Name)
            .IsRequired();

        builder
            .HasMany(x => x.Events)
            .WithMany(x => x.Teams)
            .UsingEntity<Dictionary<string, object>>("EventsTeams",
                x => x.HasOne<EventEntity>().WithMany().HasForeignKey("EventId"),
                x => x.HasOne<TeamEntity>().WithMany().HasForeignKey("TeamId"),
                x => x.ToTable("EventsTeams"));

        builder
            .HasOne(x => x.Owner);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}
