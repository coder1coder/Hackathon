using Hackathon.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class FriendOfferEntityConfiguration: IEntityTypeConfiguration<FriendshipEntity>
{
    public void Configure(EntityTypeBuilder<FriendshipEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasIndex(x => new {x.ProposerId, x.UserId}).IsUnique();
    }
}
