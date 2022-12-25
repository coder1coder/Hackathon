using Hackathon.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class EmailConfirmationRequestEntityConfiguration: IEntityTypeConfiguration<EmailConfirmationRequestEntity>
{
    public void Configure(EntityTypeBuilder<EmailConfirmationRequestEntity> builder)
    {
        builder.ToTable("EmailConfirmations");

        builder.HasKey(x => x.UserId);
    }
}
