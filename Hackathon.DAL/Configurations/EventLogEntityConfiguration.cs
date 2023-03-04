using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class EventLogEntityConfiguration: IEntityTypeConfiguration<EventLogEntity>
{
    public void Configure(EntityTypeBuilder<EventLogEntity> builder)
    {
        builder.ToTable("EventLog");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasIndex(x => x.Type);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Timestamp);
    }
}
