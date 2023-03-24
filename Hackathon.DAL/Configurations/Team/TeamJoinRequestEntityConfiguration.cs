using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.Team;

public class TeamJoinRequestEntityConfiguration: IEntityTypeConfiguration<TeamJoinRequestEntity>
{
    public void Configure(EntityTypeBuilder<TeamJoinRequestEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new {x.TeamId, x.UserId});

        builder.HasOne(x => x.Team)
            .WithMany(x => x.JoinRequests)
            .HasForeignKey(x => x.TeamId);

        builder.HasOne(x => x.User)
            .WithMany(x => x.JoinRequests)
            .HasForeignKey(x => x.UserId);
    }
}
