using Hackathon.DAL.Entities.Block;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.Block;

public class BlockingEntityConfiguration
    : IEntityTypeConfiguration<BlockingEntity>
{
    public void Configure(EntityTypeBuilder<BlockingEntity> builder)
    {
        builder.ToTable("Blocks");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.Reason)
            .IsRequired();

        builder.Property(x => x.AssignmentUserId)
            .IsRequired();

        builder.Property(x => x.TargetUserId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder
            .HasOne(x => x.TargetUser)
            .WithOne(x => x.Block)
            .HasForeignKey<BlockingEntity>(b => b.TargetUserId);
    }
}
