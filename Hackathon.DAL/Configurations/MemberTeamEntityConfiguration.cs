using Hackathon.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class MemberTeamEntityConfiguration : IEntityTypeConfiguration<MemberTeamEntity>
{
    public void Configure(EntityTypeBuilder<MemberTeamEntity> builder)
    {
        builder.ToTable("MembersTeams");

        builder
            .HasKey(t => new { t.MemberId, t.TeamId });

        builder
            .HasOne(x => x.Member)
            .WithMany(x => x.Teams)
            .HasForeignKey(fk => fk.MemberId);

        builder
            .HasOne(x => x.Team)
            .WithMany(x => x.Members)
            .HasForeignKey(fk => fk.TeamId);

        builder
            .Property(t => t.DateTimeAdd)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
    }
}
