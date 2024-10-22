using Hackathon.DAL.Entities.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.Events;

public class EventAgreementConfiguration:
    IEntityTypeConfiguration<EventAgreementEntity>,
    IEntityTypeConfiguration<EventAgreementUserEntity>
{
    public void Configure(EntityTypeBuilder<EventAgreementEntity> builder)
    {
        builder.ToTable("EventAgreements");
        builder.HasKey(x => x.EventId);

        builder
            .HasMany(x => x.Users)
            .WithMany(x => x.EventAgreements)
            .UsingEntity<EventAgreementUserEntity>(x =>
            {
                x.ToTable("EventAgreementUsers");
            });

        builder.HasOne(x => x.Event)
            .WithOne(x => x.Agreement)
            .HasForeignKey<EventAgreementEntity>(x => x.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<EventAgreementUserEntity> builder)
    {
        builder.ToTable("EventAgreementUsers");

        builder
            .HasOne(x => x.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Agreement)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
