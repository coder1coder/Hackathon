using Hackathon.DAL.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class UserReactionEntityConfiguration: IEntityTypeConfiguration<UserReactionEntity>
{
    public void Configure(EntityTypeBuilder<UserReactionEntity> builder)
    {
        builder.ToTable("UserReactions");
        builder.HasKey(x => new {x.UserId, x.TargetUserId});
    }
}
