using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Entities;

public class GoogleAccountEntity: IEntityTypeConfiguration<GoogleAccountEntity>
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string GiveName { get; set; }
    public string ImageUrl { get; set; }
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public long ExpiresAt { get; set; }
    public long ExpiresIn { get; set; }
    public long FirstIssuedAt { get; set; }
    public string TokenId { get; set; }
    public string LoginHint { get; set; }

    public int UserId { get; set; }
    public UserEntity User { get; set; }

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