using System.Collections.Generic;
using Hackathon.Common.Configuration;
using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Hackathon.DAL.Configurations
{
    public class UserEntityConfiguration: IEntityTypeConfiguration<UserEntity>
    {
        private readonly AdministratorDefaults _administratorDefaults;
        public UserEntityConfiguration(IOptions<AdministratorDefaults> administratorDefaults)
        {
            _administratorDefaults = administratorDefaults.Value;
        }
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

            if (_administratorDefaults != null)
                builder.HasData(new List<UserEntity>
                {
                    new()
                    {
                        UserName = _administratorDefaults.Login,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(_administratorDefaults.Password),
                        FullName = _administratorDefaults.Login
                    }
                });
        }
    }
}