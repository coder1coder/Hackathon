using System.Collections.Generic;
using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations
{
    public class TeamEntityConfiguration : IEntityTypeConfiguration<TeamEntity>
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
                .Property(x => x.EventId)
                .IsRequired();

            builder
                .HasMany(x => x.Users)
                .WithMany(x => x.Teams)
                .UsingEntity<Dictionary<string, object>>("UserTeam",
                x => x.HasOne<UserEntity>().WithMany().HasForeignKey("UserId"),
                x => x.HasOne<TeamEntity>().WithMany().HasForeignKey("TeamId"),
                x => x.ToTable("UserTeam"));

            builder
                .HasOne(x => x.Project)
                .WithOne(x => x.Team)
                .HasForeignKey<ProjectEntity>(x => x.TeamId);
        }
    }
}