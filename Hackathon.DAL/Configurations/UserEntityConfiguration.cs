using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations
{
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
        }
    }
}