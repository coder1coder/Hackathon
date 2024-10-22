using Hackathon.DAL.Entities.ApprovalApplications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.ApprovalApplications;

public class ApprovalApplicationConfiguration: IEntityTypeConfiguration<ApprovalApplicationEntity>
{
    public void Configure(EntityTypeBuilder<ApprovalApplicationEntity> builder)
    {
        builder.ToTable("ApprovalApplications");

        builder.Property(x => x.Comment)
            .HasMaxLength(300);

        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId);

        builder.HasOne(x => x.Signer)
            .WithMany()
            .HasForeignKey(x => x.SignerId);
    }
}
