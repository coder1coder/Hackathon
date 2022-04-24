using Hackathon.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class ProjectEntityConfiguration: IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.ToTable("Projects");

        builder
            .Property(x => x.Name)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(1000);

    }
}