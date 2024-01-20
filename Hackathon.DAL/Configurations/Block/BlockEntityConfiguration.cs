using Hackathon.DAL.Entities.Block;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.Block;

public class BlockEntityConfiguration
    : IEntityTypeConfiguration<BlockEntity>
{
    public void Configure(EntityTypeBuilder<BlockEntity> builder)
    {
        builder.ToTable("Blocks");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.Property(x => x.AdministratorId)
            .IsRequired();

        builder.Property(x => x.Reason)
            .IsRequired();

        builder.Property(x => x.IsRemove)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Blocks)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
