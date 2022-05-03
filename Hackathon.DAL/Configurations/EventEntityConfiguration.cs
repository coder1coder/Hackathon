using Hackathon.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class EventEntityConfiguration: IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.MemberRegistrationMinutes).IsRequired();
        builder.Property(x => x.DevelopmentMinutes).IsRequired();
        builder.Property(x => x.TeamPresentationMinutes).IsRequired();
        builder.Property(x => x.MinTeamMembers).IsRequired();
        builder.Property(x => x.MaxEventMembers).IsRequired();
        builder.Property(x => x.IsCreateTeamsAutomatically).IsRequired();
        builder.Property(x => x.OwnerId).IsRequired();

        builder.Property(x => x.ChangeEventStatusMessages);
    }
}