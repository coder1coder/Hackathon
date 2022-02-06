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
        private readonly AppSettings _appSettings;
        public UserEntityConfiguration(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
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

            builder
                .Property(x => x.Role)
                .HasDefaultValue(0);

            if (_appSettings.AdministratorDefaults != null)
                builder.HasData(new List<UserEntity>
                {
                    new()
                    {
                        UserName = _appSettings.AdministratorDefaults.Login,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(_appSettings.AdministratorDefaults.Password),
                        FullName = _appSettings.AdministratorDefaults.Login
                    }
                });
        }
    }
}
