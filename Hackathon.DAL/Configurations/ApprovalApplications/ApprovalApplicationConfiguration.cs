using Hackathon.DAL.Entities.ApprovalApplications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations.ApprovalApplications;

public class ApprovalApplicationConfiguration: IEntityTypeConfiguration<ApprovalApplication>
{
    public void Configure(EntityTypeBuilder<ApprovalApplication> builder)
    {
        builder.ToTable("ApprovalApplications");

        builder.Property(x => x.Comment)
            .HasMaxLength(300);

        builder.HasOne(x => x.Author)
            .WithMany();

        builder.HasOne(x => x.Event)
            .WithOne(x => x.ApprovalApplication)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
