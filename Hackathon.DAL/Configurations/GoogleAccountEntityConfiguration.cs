using Hackathon.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class GoogleAccountEntityConfiguration: IEntityTypeConfiguration<GoogleAccountEntity>
{
    public void Configure(EntityTypeBuilder<GoogleAccountEntity> builder)
    {
        builder.ToTable("GoogleAccounts");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder
            .HasOne(x => x.User)
            .WithOne(x => x.GoogleAccount);
    }
}