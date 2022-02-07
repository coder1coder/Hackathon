using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Entities
{
    public class TeamEntity : BaseEntity, IEntityTypeConfiguration<TeamEntity>
    {
        public string Name { get; set; }

        public List<TeamEventEntity> TeamEvents { get; set; } = new ();
        public List<UserEntity> Users { get; set; } = new ();
        public UserEntity Owner { get; set; }
        public long? OwnerId { get; set; }

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
                .HasMany(x => x.Users)
                .WithMany(x => x.Teams)
                .UsingEntity<Dictionary<string, object>>("UserTeam",
                    x => x.HasOne<UserEntity>().WithMany().HasForeignKey("UserId"),
                    x => x.HasOne<TeamEntity>().WithMany().HasForeignKey("TeamId"),
                    x => x.ToTable("UserTeam"));

            builder
                .HasOne(x => x.Owner);
        }
    }
}