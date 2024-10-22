using Hackathon.DAL.Entities.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.Events;

public class EventStageConfiguration: IEntityTypeConfiguration<EventStageEntity>
{
    public void Configure(EntityTypeBuilder<EventStageEntity> builder)
    {
        builder.ToTable("EventStages");

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.Stages);
    }
}
