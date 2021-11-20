using Hackathon.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations
{
    public class EventEntityConfiguration: IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.ToTable("Events");
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(x => x.Start)
                .IsRequired();

            builder
                .Property(x => x.StartMemberRegistration)
                .IsRequired();

            builder
                .Property(x => x.Status)
                .IsRequired();

            builder
                .HasMany(x => x.Teams)
                .WithOne(x => x.Event);

            builder
                .Property(x => x.MinTeamMembers)
                .IsRequired();

            builder
                .Property(x => x.MaxEventMembers)
                .IsRequired();
        }
    }
}