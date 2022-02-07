using System.Collections.Generic;
using Hackathon.Common.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Entities
{
    public class UserEntity: BaseEntity, IEntityTypeConfiguration<UserEntity>
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public UserRole Role { get; set; }

        public List<TeamEntity> Teams { get; set; } = new ();

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
        }
    }
}