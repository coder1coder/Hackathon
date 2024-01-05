using Hackathon.DAL.Entities.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.Events;

public class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
{
    private const string JsonBColumnType = "jsonb";

    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.Property(x => x.Award)
            .IsRequired();

        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.MinTeamMembers).IsRequired();
        builder.Property(x => x.MaxEventMembers).IsRequired();
        builder.Property(x => x.IsCreateTeamsAutomatically).IsRequired();
        builder.Property(x => x.OwnerId).IsRequired();

        builder.Property(x => x.ChangeEventStatusMessages)
            .HasColumnType(JsonBColumnType)
            .IsRequired();

        builder.Property(x => x.Tasks)
            .HasColumnType(JsonBColumnType)
            .IsRequired();

        builder.Property(x => x.IsDeleted).HasDefaultValue(false);

        builder.HasOne(x => x.ApprovalApplication)
            .WithOne(x => x.Event)
            .HasForeignKey<EventEntity>(s=>s.ApprovalApplicationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(x => x.Tags)
            .HasMaxLength(100);
        
        builder.HasIndex(x => x.Tags);
    }
}
