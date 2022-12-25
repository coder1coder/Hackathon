using Hackathon.Common.Models.User;
using Hackathon.Entities;
using Hackathon.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class UserEntityConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder
            .HasIndex(x => x.UserName)
            .IsUnique();

        builder
            .Property(x => x.UserName)
            .IsRequired();

        builder
            .Property(x => x.PasswordHash)
            .IsRequired();

        builder
            .Property(x => x.Role)
            .HasDefaultValue(UserRole.Default);

        builder
            .HasOne(x => x.EmailConfirmationRequest)
            .WithOne(x => x.User)
            .HasForeignKey<EmailConfirmationRequestEntity>(b => b.UserId);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);
    }
}
